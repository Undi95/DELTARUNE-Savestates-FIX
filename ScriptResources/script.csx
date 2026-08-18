using System.Linq;
using System.Threading.Tasks;
using UndertaleModLib.Util;
using ImageMagick;

string resourcePath = Path.GetDirectoryName(ScriptPath);
GlobalDecompileContext globalDecompileContext = new(Data);
Underanalyzer.Decompiler.IDecompileSettings decompileSettings = Data.ToolInfo.DecompilerSettings;

SyncBinding("Strings, Variables, Functions", true);
UndertaleModLib.Compiler.CodeImportGroup importGroup = new(Data);

CreateScriptFromResource("gml_GlobalScript_array_contains_manual");
CreateScriptFromResource("gml_GlobalScript_audio_play_sound_logged");
CreateScriptFromResource("gml_GlobalScript_ds_list_create_logged");
CreateScriptFromResource("gml_GlobalScript_sprite_get_texture_logged");
CreateScriptFromResource("gml_GlobalScript_call_later_logged");

UndertaleGameObject obj_savestate_manager;
if (Data.GameObjects.ByName("obj_savestate_manager") is null)
{
    obj_savestate_manager = new()
    {
        Name = Data.Strings.MakeString("obj_savestate_manager"),
        Persistent = true
    };
    Data.GameObjects.Add(obj_savestate_manager);
}
else
    obj_savestate_manager = Data.GameObjects.ByName("obj_savestate_manager");

// Find and replace code from FindAndReplace.csx
List<UndertaleCode> toDump = Data.Code.Where(c => c.ParentEntry is null).ToList();
await Task.Run(() =>
{
    SetProgressBar(null, "Code Entries", 0, toDump.Count);
    StartProgressBarUpdater();
    foreach (UndertaleCode code in toDump)
    {
        IncrementProgress();
        if (code is null)
            continue;
        if (code.Name.Content != "gml_GlobalScript_audio_play_sound_logged")
        {
            importGroup.QueueFindReplace(code, "audio_play_sound_at(", "audio_play_sound_at_logged(", true);
            importGroup.QueueFindReplace(code, "audio_play_sound(", "audio_play_sound_logged(", true);
            importGroup.QueueFindReplace(code, "audio_stop_sound(", "audio_stop_sound_logged(", true);
            importGroup.QueueFindReplace(code, "audio_create_stream(", "audio_create_stream_logged(", true);
            importGroup.QueueFindReplace(code, "audio_destroy_stream(", "audio_destroy_stream_logged(", true);
            importGroup.QueueFindReplace(code, "audio_sound_gain(", "audio_sound_gain_logged(", true);
            importGroup.QueueFindReplace(code, "audio_sound_pitch(", "audio_sound_pitch_logged(", true);
        }
        if (code.Name.Content != "gml_GlobalScript_ds_list_create_logged")
        {
            importGroup.QueueFindReplace(code, "ds_list_create(", "ds_list_create_logged(", true);
            importGroup.QueueFindReplace(code, "ds_map_create(", "ds_map_create_logged(", true);
        }
        if (code.Name.Content != "gml_GlobalScript_sprite_get_texture_logged")
            importGroup.QueueFindReplace(code, "sprite_get_texture(", "sprite_get_texture_logged(", true);
        if (code.Name.Content != "gml_GlobalScript_call_later_logged")
            importGroup.QueueFindReplace(code, "call_later(", "call_later_logged(", true);
    }
    SetProgressBar(null, "Adding savestate manager", toDump.Count, toDump.Count);
});

if (Data.GameObjects.ByName("obj_nothing") is null)
{
    UndertaleGameObject obj_nothing = new()
    {
        Name = Data.Strings.MakeString("obj_nothing")
    };
    Data.GameObjects.Add(obj_nothing);
}

if (Data.Sprites.ByName("spr_sneo_playback") is null)
{
    // Sprite import code from ImportGraphics.csx
    // I was too lazy and hardcoded it...
    int lastTextPage = Data.EmbeddedTextures.Count - 1;
    UndertaleEmbeddedTexture texture = new()
    {
        Name = Data.Strings.MakeString($"Texture {++lastTextPage}")
    };
    using MagickImage textureImage = TextureWorker.ReadBGRAImageFromFile(Path.Combine(resourcePath, "spr_sneo_playback.png"));
    texture.TextureData.Image = GMImage.FromMagickImage(textureImage).ConvertToPng();
    Data.EmbeddedTextures.Add(texture);
    int lastTextPageItem = Data.TexturePageItems.Count - 1;
    UndertaleTexturePageItem wreck = new()
    {
        Name = Data.Strings.MakeString($"PageItem {++lastTextPageItem}"),
        SourceX = (ushort)0,
        SourceY = (ushort)0,
        SourceWidth = (ushort)166,
        SourceHeight = (ushort)32,
        TargetX = (ushort)5,
        TargetY = (ushort)5,
        TargetWidth = (ushort)166,
        TargetHeight = (ushort)32,
        BoundingWidth = (ushort)186,
        BoundingHeight = (ushort)40,
        TexturePage = texture
    };
    UndertaleTexturePageItem pause = new()
    {
        Name = Data.Strings.MakeString($"PageItem {++lastTextPageItem}"),
        SourceX = (ushort)0,
        SourceY = (ushort)34,
        SourceWidth = (ushort)143,
        SourceHeight = (ushort)30,
        TargetX = (ushort)7,
        TargetY = (ushort)7,
        TargetWidth = (ushort)143,
        TargetHeight = (ushort)30,
        BoundingWidth = (ushort)186,
        BoundingHeight = (ushort)40,
        TexturePage = texture
    };
    UndertaleTexturePageItem spew = new()
    {
        Name = Data.Strings.MakeString($"PageItem {++lastTextPageItem}"),
        SourceX = (ushort)0,
        SourceY = (ushort)66,
        SourceWidth = (ushort)136,
        SourceHeight = (ushort)31,
        TargetX = (ushort)3,
        TargetY = (ushort)6,
        TargetWidth = (ushort)136,
        TargetHeight = (ushort)31,
        BoundingWidth = (ushort)186,
        BoundingHeight = (ushort)40,
        TexturePage = texture
    };
    Data.TexturePageItems.Add(wreck);
    Data.TexturePageItems.Add(pause);
    Data.TexturePageItems.Add(spew);

    UndertaleSprite spr_sneo_playback = new()
    {
        Name = Data.Strings.MakeString("spr_sneo_playback"),
        Width = 186u,
        Height = 40u,
        MarginLeft = 3,
        MarginRight = 170,
        MarginBottom = 36,
        MarginTop = 5,
        OriginX = 0,
        OriginY = 0,
    };
    UndertaleSprite.TextureEntry wreckEntry = new() { Texture = wreck };
    UndertaleSprite.TextureEntry pauseEntry = new() { Texture = pause };
    UndertaleSprite.TextureEntry spewEntry = new() { Texture = spew };
    spr_sneo_playback.Textures.Add(wreckEntry);
    spr_sneo_playback.Textures.Add(pauseEntry);
    spr_sneo_playback.Textures.Add(spewEntry);
    Data.Sprites.Add(spr_sneo_playback);
}
;

AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Create_0", EventType.Create);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Alarm_0", EventType.Alarm, 0u);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Step_0", EventType.Step, (uint)EventSubtypeStep.Step);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Step_1", EventType.Step, (uint)EventSubtypeStep.BeginStep);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Step_2", EventType.Step, (uint)EventSubtypeStep.EndStep);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Draw_64", EventType.Draw, (uint)EventSubtypeDraw.DrawGUI);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Draw_75", EventType.Draw, (uint)EventSubtypeDraw.DrawGUIEnd);
// Post-Draw (and the general pause code) was copied over from https://www.youtube.com/watch?v=dNiLIX8jNOM 
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_Draw_77", EventType.Draw, (uint)EventSubtypeDraw.PostDraw);
AddEventFromResource(obj_savestate_manager, "gml_Object_obj_savestate_manager_CleanUp_0", EventType.CleanUp);

UndertalePointerList<UndertaleRoom.GameObject> roomGameObjects = Data.GeneralInfo.RoomOrder.First().Resource.GameObjects;
if (!roomGameObjects.Any(inst => inst.ObjectDefinition.Name?.Content == "obj_savestate_manager"))
{
    roomGameObjects.Insert(0, new UndertaleRoom.GameObject()
    {
        InstanceID = Data.GeneralInfo.LastObj++,
        ObjectDefinition = obj_savestate_manager,
        X = 0,
        Y = 0
    });
}
await Task.Run(() => UpdateProgressStatus("Final code import..."));
importGroup.Import();
DisableAllSyncBindings();

StopProgressBarUpdater();
HideProgressBar();
ScriptMessage("Savestates imported! Use the 1-9 keys to change savestate slot, S to save, and L to load. Hold Tab to see what rooms your savestates are in.");

UndertaleCode CreateCodeEntryFromResource(string filename)
{
    string fullPath = Path.Combine(resourcePath, filename) + ".gml";
    importGroup.QueueReplace(filename, File.ReadAllText(fullPath));
    return Data.Code.ByName(filename);
}

UndertaleScript CreateScriptFromResource(string filename)
{
    UndertaleCode codeEntry = CreateCodeEntryFromResource(filename);
    UndertaleScript? script = Data.Scripts.ByName(Data.Code.ByName(filename).Name.Content);
    if (script is null)
    {
        script = new UndertaleScript() { Name = Data.Code.ByName(filename).Name, Code = codeEntry};
        Data.Scripts.Add(script);
    }
    return script;
}

UndertaleCode AddEventFromResource(UndertaleGameObject gameObject, string filename, EventType eventType, uint subtype)
{
    string fullPath = Path.Combine(resourcePath, filename) + ".gml";
    importGroup.QueueReplace(gameObject.EventHandlerFor(eventType, subtype, Data), File.ReadAllText(fullPath));
    return Data.Code.ByName(filename);
}

UndertaleCode AddEventFromResource(UndertaleGameObject gameObject, string filename, EventType eventType)
{
    return AddEventFromResource(gameObject, filename, eventType, 0u);
}
function sprite_get_texture_logged(arg0, arg1)
{
    var texture = sprite_get_texture(arg0, arg1);
    
    with (obj_savestate_manager)
    {
        variable_struct_set(known_textures, string(texture), 
        {
            spr: arg0,
            subimg: arg1
        });
    }
    
    return texture;
}

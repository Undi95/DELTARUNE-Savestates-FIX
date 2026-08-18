var msg_start_y = 0;
var old_alpha = draw_get_alpha();
var old_font = draw_get_font();
var old_color = draw_get_color();
var old_halign = draw_get_halign();
draw_set_font(fnt_main);
draw_set_color(c_white);
draw_set_halign(fa_left);

if (msg_opacity > 0)
{
    draw_set_alpha(msg_opacity);
    draw_text(10, 10, debug_msg);
    msg_start_y += 15;
    msg_opacity -= 0.05;
}

if (keyboard_check(vk_tab))
{
    draw_set_alpha(1);
    
    for (var i = 0; i < 10; i++)
    {
        var file_id = file_text_open_read(game_save_id + "Savestates/Chapter " + string(global.chapter) + "/" + string(i) + "/room.txt");
        var room_name = "N/A";
        
        if (file_id != -1)
        {
            var room_id = file_text_readln(file_id);
            room_name = room_get_name(real(room_id));
            file_text_close(file_id);
        }
        
        draw_text(10, msg_start_y + 10 + (15 * i), "Savestate " + string(i) + ":  " + room_name);
    }
}

draw_set_alpha(old_alpha);
draw_set_font(old_font);
draw_set_color(old_color);
draw_set_halign(old_halign);

if (pause)
    draw_sprite(spr_sneo_playback, 1, 0, display_get_gui_height() - sprite_get_height(spr_sneo_playback));

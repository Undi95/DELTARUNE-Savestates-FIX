function ds_list_create_logged()
{
    var list = ds_list_create();
    
    with (obj_savestate_manager)
    {
        if (ds_max_id.list < list)
            ds_max_id.list = list;
    }
    
    return list;
}

function ds_map_create_logged()
{
    var map = ds_map_create();
    
    with (obj_savestate_manager)
    {
        if (ds_max_id.map < map)
            ds_max_id.map = map;
    }
    
    return map;
}

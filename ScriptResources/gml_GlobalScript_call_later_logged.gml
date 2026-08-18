function call_later_logged(arg0, arg1, arg2, arg3 = false)
{
    var call_id = call_later(arg0, arg1, arg2, arg3);
    
    with (obj_savestate_manager)
    {
        array_push(known_call_laters, 
        {
            period: arg0,
            unit: arg1,
            callback: arg2,
            loop: arg3,
            time: current_time,
            id: call_id
        });
    }
    
    return call_id;
}

ScutScene = {
    root=nil
}

print(ScutScene)

function ScutScene:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function ScutScene:create_and_init()
    root = CCScene:create()
    self:register_script_handler()
    return root
end

function ScutScene:on_lua_scene_enter()
    if self.on_enter then
        self:on_enter()
    end
end

function ScutScene:on_lua_scene_exit()
    if self.on_exit then
        self:on_exit()
    end
end

function ScutScene:register_script_handler()
    local function on_node_event(event)
        if event == "enter" then
            self:on_lua_scene_enter()
        elseif event == "exit" then
            self:on_lua_scene_exit()
        end
    end
    self.root:registerScriptHandler(on_node_event)
end

function ScutScene:register_callback()
end

function ScutScene:exec_callback()
end

function ScutScene:net_error_func()
    end

function ScutScene:net_common_data_func()
end

function ScutScene:net_decode_end_func()
end
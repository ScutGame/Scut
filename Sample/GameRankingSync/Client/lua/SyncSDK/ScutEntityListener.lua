--ScutEntityListener.lua
module("ScutEntityListener", package.seeall)

function NotifySceneLayer(pScutScene, lpExternalData, schemaName)

    local content = "hello..."
    local root = ScutEntityFactory(schemaName)
    local entity = root:first()
    if schemaName == "UserRanking" then
    
    else
    
    end
    
    ZyToast.show(pScutScene, content)
end
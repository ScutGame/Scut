------------------------------------------------------------------
-- ScutSchemaInfo.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: The schema of entity info.
------------------------------------------------------------------
module("ScutSchemaInfo", package.seeall)
--The following code is automatically generated
g__schema = {}
g__schema["UserRanking"] = {
    ["UserID"] = {1, "int", true},
    ["UserName"] = {2, "string"},
    ["Score"] = {3, "int"},
    ["CreateDate"] = {4, "datetime"}
}



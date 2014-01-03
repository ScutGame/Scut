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
g__schema["GameUser"] = {
    ["UserID"] = {1, "int", true},
    ["LoginMap"] = {2, "int"},
    ["HeadId"] = {3, "string"},
    ["NickName"] = {4, "string"},
    ["Sex"] = {5, "bool"},
    ["Profession"] = {6, "int"},
    ["UserLv"] = {7, "short"},
    ["VipLv"] = {8, "int"},
    ["GiftGold"] = {9, "int"},
    ["PayGold"] = {10, "int"},
    ["ExtGold"] = {11, "int"},
    ["UseGold"] = {12, "int"},
    ["BUseGold"] = {13, "int"},
    ["GameCoin"] = {14, "int"},
    ["BGameCoin"] = {15, "int"},
    ["SGameCoin"] = {16, "int"},
    ["Pid"] = {17, "string"},
    ["RetailID"] = {18, "string"},
    ["DeviceID"] = {19, "string"},
    ["UserStatus"] = {20, "int"},
    ["MsgState"] = {21, "bool"},
    ["MobileType"] = {22, "int"},
    ["ScreenX"] = {23, "short"},
    ["ScreenY"] = {24, "short"},
    ["ClientAppVersion"] = {25, "string"},
    ["LoginTime"] = {26, "datetime"},
    ["CreateDate"] = {27, "datetime"},
    ["IBagNum"] = {28, "int"},
    ["MBagNum"] = {29, "int"},
    ["TBagNum"] = {30, "int"},
    ["SBagNum"] = {31, "int"},
    ["PetNum"] = {32, "int"},
    ["TeachLv"] = {33, "int"},
    ["EthicsNum"] = {34, "int"},
    ["BetrayDate"] = {35, "datetime"},
    ["BrotherID"] = {36, "string"},
    ["MarryID"] = {37, "string"},
    ["FactionID"] = {38, "string"},
    ["StatusTime"] = {39, "datetime"},
    ["Honor"] = {40, "int"},
    ["TechUserID"] = {41, "int"},
    ["KillCount"] = {42, "int"},
    ["Murderous"] = {43, "int"},
    ["ActionStatus"] = {44, "int"},
    ["ProContribute"] = {45, "int"},
    ["HangUpList"] = {
        46,
        ["_hasChild"] = true,
        ["_list"] = {
            1,
            ["_hasChild"] = true,
            ["HangUpType"] = {1, "int"},
            ["HangUpStatus"] = {2, "int"}
        }
    },
    ["RegionList"] = {
        47,
        ["_hasChild"] = true,
        ["_list"] = {1, "int"}
    },
    ["UserFC"] = {48, "int"},
    ["FactionMap"] = {49, "int"},
    ["X"] = {50, "int"},
    ["Y"] = {51, "int"},
    ["PContributeNum"] = {52, "int"}
}


g__schema["UserItem"] = {
    ["UserID"] = {1, "int", true},
    ["ItemInfoPackage"] = {
        2,
        ["_hasChild"] = true,
        ["_list"] = {
            1,
            ["_hasChild"] = true,
            ["UserItemID"] = {1, "string"},
            ["ItemID"] = {2, "int"},
            ["Num"] = {3, "int"},
            ["Level"] = {4, "int"},
            ["ItemInType"] = {5, "int"},
            ["IsCarve"] = {6, "bool"},
            ["IsBind"] = {7, "bool"},
            ["UserEqui"] = {
                8,
                ["_hasChild"] = true,
                ["Endurance"] = {1, "int"},
                ["Resupply"] = {2, "int"},
                ["ForgeLv"] = {3, "int"},
                ["GemID"] = {4, "int"},
                ["GemNum"] = {5, "int"},
                ["Addition"] = {
                    6,
                    ["_hasChild"] = true,
                    ["_list"] = {
                        1,
                        ["_hasChild"] = true,
                        ["AttrType"] = {1, "int"},
                        ["AttrNum"] = {2, "int"},
                        ["PreNum"] = {3, "decimal"},
                        ["BaseAttrNum"] = {4, "int"}
                    }
                },
                ["PetID"] = {7, "string"},
                ["Special"] = {8, "int"},
                ["PourID"] = {9, "int"},
                ["PourLv"] = {10, "int"}
            }
        }
    }
}



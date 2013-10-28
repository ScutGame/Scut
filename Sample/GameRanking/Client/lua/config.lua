
-- 0 - disable debug info, 1 - less debug info, 2 - verbose debug info
DEBUG = 2
DEBUG_FPS = true
--DEBUG_MEM = true

-- design resolution
CONFIG_SCREEN_WIDTH  = 960
CONFIG_SCREEN_HEIGHT = 640

-- auto scale mode
CONFIG_SCREEN_AUTOSCALE = "FIXED_HEIGHT"

-- sounds
GAME_SFX = {
    tapButton  = "audio/TapButtonSound.mp3",
    backButton = "audio/BackButtonSound.mp3",
    flipCoin = "audio/ConFlipSound.mp3",
    levelCompleted = "audio/LevelWinSound.mp3",
}

SEARCH_PATH = {"animations", "images", "fonts", "particles", "test/coinflip"}

ANIMATIONS = {
    {image_name="hero1000.png",     plist_name="hero1000.plist",     config_name="hero1000.xml",           actor_name="hero1000",    actor_scale=1},
    {image_name="MonsterZero0.png", plist_name="MonsterZero0.plist", config_name="MonsterZero.ExportJson", actor_name="MonsterZero", actor_scale=1},
    {image_name="Dragon.png",       plist_name="Dragon.plist",       config_name="Dragon.xml",             actor_name="Dragon",      actor_scale=1},
    {image_name="cyborg.png",       plist_name="cyborg.plist",       config_name="cyborg.xml",             actor_name="cyborg",      actor_scale=1},
    {image_name="robot.png",        plist_name="robot.plist",        config_name="robot.xml",              actor_name="robot",       actor_scale=1},
    {image_name="knight.png",       plist_name="knight.plist",       config_name="knight.xml",             actor_name="Knight_f/Knight",       actor_scale=2},
    {image_name="TestBone0.png",    plist_name="TestBone0.plist",    config_name="TestBone.json",          actor_name="TestBone",    actor_scale=0.5},
    {image_name="monster1000.png",  plist_name="monster1000.plist",  config_name="monster1000.xml",        actor_name="monster1000", actor_scale=1},
    {image_name="effect100.png",    plist_name="effect100.plist",    config_name="effect100.xml",          actor_name="effect100",   actor_scale=1},
    {image_name="effect101.png",    plist_name="effect101.plist",    config_name="effect101.xml",          actor_name="effect101",   actor_scale=1},
	{image_name="effect102.png",    plist_name="effect102.plist",    config_name="effect102.xml",          actor_name="effect102",   actor_scale=1},
    {image_name="effect103.png",    plist_name="effect103.plist",    config_name="effect103.xml",          actor_name="effect103",   actor_scale=1},
    {image_name="effect104.png",    plist_name="effect104.plist",    config_name="effect104.xml",          actor_name="effect104",   actor_scale=1},
}

WEAPONS = {
    {image_name="weapon.png",       plist_name="weapon.plist",       config_name="weapon.xml",             actor_name="weapon",      actor_scale=0.5},
}

ITEMS = {10000,20001,10001,}

PARTICLES = 
{
"BoilingFoam.plist",
"BurstPipe.plist",
"Comet.plist",
"debian.plist",
"SpinningPeas.plist",
"ExplodingRing.plist",
"LavaFlow.plist",
"Upsidedown.plist",
"SpookyPeas.plist",
"Phoenix.plist",
}

SPRITE_FRAMES_FILE = 
{
    ['common']=
    {
        {data_file="CoinFlip.plist",         image_file="CoinFlip.png"},
        {data_file="pd_sprites.plist",       image_file="ss.png"}
    },
    ['home']={{data_file="home.plist",   image_file="home.png"}},
    ['main']={},
    ['test']={{data_file="AllSprites.plist",       image_file="AllSprites.png"},},
}


arale_global = 
{
    hero_weapons = 
    {
    "weapon_f-sword.png",
    "weapon_f-sword2.png",
    "weapon_f-sword3.png",
    "weapon_f-sword4.png",
    "weapon_f-sword5.png",
    "weapon_f-knife.png",
    "weapon_f-hammer.png"
    },

}
COIN_FLIP_IMAGE_FILENAME = "CoinFlip.png"
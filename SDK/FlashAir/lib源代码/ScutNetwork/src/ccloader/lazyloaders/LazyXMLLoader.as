package ccloader.lazyloaders{
    import ccloader.*;
    import ccloader.loadingtypes.*;
    import ccloader.lazyloaders.*;
    import flash.events.*;
    import flash.net.*;
    import flash.display.*;
    import flash.media.Sound;
    import flash.utils.*;
    import flash.system.LoaderContext;
    import flash.system.ApplicationDomain;
    import flash.media.SoundLoaderContext;

    /**
     *       @example Basic usage:<listing version="3.0">   
     var lazy : LazyXMLLoader = new LazyXMLLoader("sample-lazy.xml", "myCCLoader");
    // listen to when the lazy loader has loaded the external definition
    lazy.addEventListener(Event.LAZY_LOADED, onLazyLoaded);
    // add regular events to the CCLoader instance
    lazy.addEventListener(ProgressEvent.PROGRESS, onLazyProgress);
    lazy.addEventListener(Event.LAZY_LOADED, onAllItemsLoaded);

    function onLazyLoaded(evt : Event) : void{
    // now you can add individual events for items
    onLazyLoaded.get("config").addEventListener(CCLoader.COMPLETE, onConfigLoaded);
    ...
    }
    </listing>
     */
    dynamic public class LazyXMLLoader extends LazyCCLoader {
        function LazyXMLLoader(url : *, name : String, numConnections : int = CCLoader.DEFAULT_NUM_CONNECTIONS, logLevel : int = CCLoader.DEFAULT_LOG_LEVEL){
            super (url, name, numConnections, logLevel);
        }
        public var _sourceXML : XML;

        /** Reads a xml as a string and create a complete bulk loader from it.
         *   @param withData The xml to be read as a string.
         *   @private
         */
        lazy_loader override function _lazyParseLoader(withData : String) : void{
            var xml : XML = _sourceXML = new XML(withData);
            var substitutions : Object = stringSubstitutions ||  {};
            for each (var substitutionXML: *in xml.stringSubstitutions.children()){
                substitutions[substitutionXML.name()] = substitutionXML.toString();
            }
            stringSubstitutions = substitutions;
            allowsAutoIDFromFileName = lazy_loader::toBoolean(xml.allowsAutoIDFromFileName);
            var possibleHandlerName : String;
            var theNode : XMLList;
            var hasNode : Boolean;
            for each (var itemNode:XML in xml.files.children()) {
                var props : Object = {};
                var atts : XMLList = itemNode.@*;
                var nodeName : String;
                var headers : Array;
                var header : URLRequestHeader;
                var headerName : String;
                var headerValue : String;
                if (!String(itemNode.url)) {
                    trace("[LazyBulkLoader] got a item files with no url, ignoring");
                    continue;
                }
                for each (var configNode:XML in itemNode.children()) {
                    nodeName = configNode.name();
                    if (nodeName == "headers") {
                        headers = [];
                        for each (var headerNode:XML in configNode.children()) {
                            headerName = String(headerNode.name());
                            headerValue = String(headerNode[0]) ;
                            header = new URLRequestHeader( headerName, headerValue);
                            headers.push(header);
                        }
                        props["headers"] = headers;
                    } else if (nodeName == "context") {
                        // todo: catch for sound items
                        var context : Object;
                        if (CCLoader.guessType(String(itemNode.url)) == CCLoader.TYPE_SOUND) {
                            context = new SoundLoaderContext();
                        } else {
                            context = new LoaderContext();
                        }
                        context.applicationDomain = ApplicationDomain.currentDomain;
                        props[CCLoader.CONTEXT] = context;

                    } else if (lazy_loader::INT_TYPES.indexOf(nodeName) > -1) {
                        props[nodeName] = int(String(configNode));
                        //trace("(is int)");
                    } else if (lazy_loader::NUMBER_TYPES.indexOf(nodeName) > -1) {
                        props[nodeName] = Number(String(configNode));
                        //trace("(is number)");
                    } else if (lazy_loader::STRINGED_BOOLEAN.indexOf(nodeName) > -1) {
                        props[nodeName] = lazy_loader::toBoolean(String(configNode));
                        //trace("(is boolean)");
                    } else if (nodeName != "url") {
                        props[nodeName] = String(configNode);
                    }
                }
                var theItem : LoadingItem = add(String(String(itemNode.url)), props);
            }
        }
    }
}

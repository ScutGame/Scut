package ccloader.loadingtypes {
    import ccloader.loadingtypes.LoadingItem;
    import ccloader.CCLoader;
    import flash.display.*;
    import flash.net.*;
    import flash.events.*;
    import flash.utils.*;

    /** @private */
    public class XMLItem extends LoadingItem {
        public var loader : URLLoader;

        public function XMLItem(url : URLRequest, type : String, uid : String){
            super(url, type, uid);
        }

        override public function _parseOptions(props : Object)  : Array{
            return super._parseOptions(props);
        }

        override public function load() : void{
            super.load();
            loader = new URLLoader();
            loader.addEventListener(ProgressEvent.PROGRESS, onProgressHandler, false, 0, true);
            loader.addEventListener(Event.COMPLETE, onCompleteHandler, false, 0, true);
            loader.addEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false, 0, true);
            loader.addEventListener(HTTPStatusEvent.HTTP_STATUS, super.onHttpStatusHandler, false, 0, true);
            loader.addEventListener(SecurityErrorEvent.SECURITY_ERROR, super.onSecurityErrorHandler, false, 0, true);
            loader.addEventListener(Event.OPEN, onStartedHandler, false, 0, true);
            try{
                // TODO: test for security error thown.
                loader.load(url);
            }catch( e : SecurityError){
                onSecurityErrorHandler(_createErrorEvent(e));

            }
        };

        override public function onErrorHandler(evt : ErrorEvent) : void{
            super.onErrorHandler(evt);
        }

        override public function onStartedHandler(evt : Event) : void{
            super.onStartedHandler(evt);
        };

        override public function onCompleteHandler(evt : Event) : void {
            try{
                _content = new XML(loader.data);
            }catch(e  : Error){
                _content = null;
                status = STATUS_ERROR;  
                dispatchEvent(_createErrorEvent(e));
            }
            super.onCompleteHandler(evt);
        };

        override public function stop() : void{
            try{
                if(loader){
                    loader.close();
                }
            }catch(e : Error){

            }
            super.stop();
        };

        override public function cleanListeners() : void {
            if(loader){
                loader.removeEventListener(ProgressEvent.PROGRESS, onProgressHandler, false);
                loader.removeEventListener(Event.COMPLETE, onCompleteHandler, false);
                loader.removeEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false);
                loader.removeEventListener(CCLoader.OPEN, onStartedHandler, false);
                loader.removeEventListener(HTTPStatusEvent.HTTP_STATUS, super.onHttpStatusHandler, false);
                loader.removeEventListener(SecurityErrorEvent.SECURITY_ERROR, super.onSecurityErrorHandler, false);
            }

        }

        override public function isText(): Boolean{
            return true;
        }

        override public function destroy() : void{
            stop();
            cleanListeners();
            _content = null;
            loader = null;
        }
    }
}

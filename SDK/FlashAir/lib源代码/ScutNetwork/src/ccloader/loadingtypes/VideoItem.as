package ccloader.loadingtypes {

    import ccloader.CCLoader;

    import flash.display.*;
    import flash.events.*;
    import flash.net.*;
    import flash.utils.*;

    /** @private */
    public class VideoItem extends LoadingItem {

        // for video:
        private var nc:NetConnection;

        /**
         *   @private
         */
        public var stream : NetStream;
        /**
         *   @public
         */
        public var dummyEventTrigger : Sprite;

        /**
         *   @private
         */
        public var _checkPolicyFile : Boolean;
        /**
         *   @private
         */
        public var pausedAtStart : Boolean = false;
        /**
         *   @private
         */
        public var _metaData : Object;

        /** Indicates if we've already fired an event letting users know that the netstream can
         *   begin playing (has enough buffer to play with no interruptions)
         *   @private
         */
        public var _canBeginStreaming : Boolean = false;

        public function VideoItem(url : URLRequest, type : String, uid : String){
            specificAvailableProps = [CCLoader.CHECK_POLICY_FILE, CCLoader.PAUSED_AT_START];
            super(url, type, uid);
            // apparently, if the stream is a mp4 file, this value beings as -1! issue 57
            _bytesTotal = _bytesLoaded = 0;
        }

        override public function _parseOptions(props : Object)  : Array{
            pausedAtStart = props[CCLoader.PAUSED_AT_START] || false;
            _checkPolicyFile = props[CCLoader.CHECK_POLICY_FILE] || false;
            return super._parseOptions(props);
        }

        override public function load() : void{
            super.load();
            nc = new NetConnection();
            nc.connect(null);
            stream = new NetStream(nc);
            stream.addEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false, 0, true);
            stream.addEventListener(NetStatusEvent.NET_STATUS, onNetStatus, false, 0, true);
            dummyEventTrigger = new Sprite();
            dummyEventTrigger.addEventListener(Event.ENTER_FRAME, createNetStreamEvent, false, 0, true);
            var customClient:Object = new Object();
            customClient.onCuePoint = function(...args):void{};
            customClient.onMetaData = onVideoMetadata;
            customClient.onPlayStatus = function(...args):void{};
            stream.client = customClient;

            try{
                // TODO: test for security error thown.
                stream.play(url.url, _checkPolicyFile);
            }catch( e : SecurityError){
                onSecurityErrorHandler(_createErrorEvent(e));
            }
        };

        /**
         *   @private
         */
        public function createNetStreamEvent(evt : Event) : void{
            if(_bytesTotal == _bytesLoaded && _bytesTotal > 8){
                // done loading: clean up, trigger on complete
                if (dummyEventTrigger) dummyEventTrigger.removeEventListener(Event.ENTER_FRAME, createNetStreamEvent, false);
                // maybe the video is in cache, and we need to trigger CAN_BEGIN_PLAYING:
                fireCanBeginStreamingEvent();
                var completeEvent : Event = new Event(Event.COMPLETE);
                onCompleteHandler(completeEvent);
            }else if(_bytesTotal == 0 && stream && stream.bytesTotal > 4){
                // just sa
                var startEvent : Event = new Event(Event.OPEN);
                onStartedHandler(startEvent);
                _bytesLoaded = stream.bytesLoaded;
                _bytesTotal = stream.bytesTotal;

            }else if (stream){
                var event : ProgressEvent = new ProgressEvent(ProgressEvent.PROGRESS, false, false, stream.bytesLoaded, stream.bytesTotal);
                // if it's a video, check if we predict that time until finish loading
                // is enough to play video back

                if (isVideo()  && metaData && !_canBeginStreaming){
                    var timeElapsed : int = getTimer() - responseTime;
                    // se issue 49 on this hack
                    if (timeElapsed > 100){
                        var currentSpeed : Number = bytesLoaded / (timeElapsed/1000);
                        // calculate _bytes remaining, before the super onProgressHandler fires
                        _bytesRemaining = _bytesTotal - bytesLoaded;
                        // be cautios, give a 20% error margin for estimated download time:
                        var estimatedTimeRemaining : Number = _bytesRemaining / (currentSpeed * 0.8);
                        var videoTimeToDownload : Number = metaData.duration - stream.bufferLength;
                        if (videoTimeToDownload > estimatedTimeRemaining){
                            fireCanBeginStreamingEvent();
                        }
                    }

                }
                super.onProgressHandler(event)
            }
        }

        override public function onCompleteHandler(evt : Event) : void {
            _content = stream;
            super.onCompleteHandler(evt);
        };

        override public function onStartedHandler(evt : Event) : void{
            _content = stream;
            if(pausedAtStart && stream){
                stream.pause();
                stream.seek(0);
            };
            super.onStartedHandler(evt);
        };

        override public function stop() : void{
            try{
                if(stream){
                    stream.close();
                }
            }catch(e : Error){

            }
            super.stop();
        };

        override public function cleanListeners() : void {
            if (stream) {
                stream.removeEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false);
                stream.removeEventListener(NetStatusEvent.NET_STATUS, onNetStatus, false);

            }
            if(dummyEventTrigger){
                dummyEventTrigger.removeEventListener(Event.ENTER_FRAME, createNetStreamEvent, false);
                dummyEventTrigger = null;
            }
        }

        override public function isVideo(): Boolean{
            return true;
        }
        override public function isStreamable() : Boolean{
            return true;
        }

        override public function destroy() : void{
            if(stream){
                //stream.client = null;
            }
            stop();
            cleanListeners();
            stream = null;
            super.destroy();
        }

        /**
         *   @private
         */
        internal function onNetStatus(evt : NetStatusEvent) : void{
            if(!stream){
                return;
            }
            stream.removeEventListener(NetStatusEvent.NET_STATUS, onNetStatus, false);
            if(evt.info.code == "NetStream.Play.Start"){
                _content = stream;
                var e : Event = new Event(Event.OPEN);
                onStartedHandler(e);
            }else if(evt.info.code == "NetStream.Play.StreamNotFound"){
                onErrorHandler(_createErrorEvent(new Error("[VideoItem] NetStream not found at " + this.url.url)));
            }
        }

        /**
         *   @private
         */
        internal function onVideoMetadata(evt : *):void{
            _metaData = evt;
        };

        /**
         *   @private
         */
        public function get metaData() : Object { 
            return _metaData; 
        }

        public function get checkPolicyFile() : Object { 
            return _checkPolicyFile; 
        }

        private function fireCanBeginStreamingEvent() : void{
            if(_canBeginStreaming){
                return;
            }
            _canBeginStreaming = true;
            var evt : Event = new Event(CCLoader.CAN_BEGIN_PLAYING);
            dispatchEvent(evt);
        }

        public function get canBeginStreaming() : Boolean{
            return _canBeginStreaming;
        }
    }
}

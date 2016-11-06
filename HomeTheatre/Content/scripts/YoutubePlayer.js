// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX
// A helper function that loads the Youtube API. It will make sure the API
// is only loaded once even if it's called multiple times.
var loadAPI = (function () {
    var status = null;
    var callbacks = [];
    function onload() {
        status = 'loaded';
        while (callbacks.length) {
            callbacks.shift()();
        }
    }
    return function (callback) {
        if (status === 'loaded') {
            setTimeout(callback, 0);
            return;
        }
        callbacks.push(callback);
        if (status === 'loading') {
            return;
        }
        status = 'loading';
        var script = document.createElement('script');
        script.src = 'https://www.youtube.com/iframe_api';
        onYouTubeIframeAPIReady = onload;
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(script, firstScriptTag);
    };
})();
var PLAYER_ID_PREFIX = 'ytplayer';
var playerSeqID = 0;
var YoutubePlayer = React.createClass.apply(React, [{
    propTypes: {
        autoplay: React.PropTypes.bool,
        theme: React.PropTypes.oneOf(['dark', 'light']),
        width: React.PropTypes.number,
        height: React.PropTypes.number,
        videoID: React.PropTypes.string.isRequired,
        onSwitchVideo: React.PropTypes.func,
        // Player events:
        onPlay: React.PropTypes.func,
        onPause: React.PropTypes.func,
        onBuffering: React.PropTypes.func,
        onEnd: React.PropTypes.func,
    },
    getDefaultProps: function () {
        return {
            autoplay: true,
            theme: 'dark',
            width: 640,
            height: 390,
            onSwitchVideo: function () { },
        };
    },
    /* Public API for controlling the player */
    play: function () {
        this.player && this.player.playVideo();
    },
    pause: function () {
        this.player && this.player.pauseVideo();
    },
    mute: function () {
        this.player && this.player.mute();
    },
    unMute: function () {
        this.player && this.player.unMute();
    },
    /* Life cycle methods */
    componentWillMount: function () {
        this.playerID = PLAYER_ID_PREFIX + String(playerSeqID++);
        loadAPI(this._onAPIReady);
    },
    componentWillUnmount: function () {
        this.player && this.player.destroy();
    },
    componentWillReceiveProps: function (_a) {
        var videoID = _a.videoID;
        this.player && this._maybeUpdatePlayer(videoID);
    },
    _maybeUpdatePlayer: function (videoID) {
        if (this.player.getVideoData().video_id !== videoID) {
            // If it's a different video => stop current video + load the new one!
            // (loading will automatically play the video)
            this.player.stopVideo();
            this.player.loadVideoById(videoID);
        }
    },
    render: function () {
        var _a = void 0, autoplay = _a.autoplay, videoID = _a.videoID, width = _a.width, height = _a.height, theme = _a.theme;
    } }].concat(other));
this.props;
return React.createElement("div", React.__spread({}, other, {id: this.playerID}));
/* Handling the API and the player */
_onAPIReady();
{
    if (!this.isMounted()) {
        return;
    }
    var _a = this.props, autoplay = _a.autoplay, videoID = _a.videoID, width = _a.width, height = _a.height, theme = _a.theme;
    new YT.Player(this.playerID, {
        width: String(width),
        height: String(height),
        videoId: videoID,
        playerVars: {
            // `autoplay` only accepts 1 or 0
            autoplay: autoplay ? 1 : 0,
            theme: theme,
        },
        events: {
            onReady: this._onPlayerReady,
            onStateChange: this._onPlayerStateChange,
        },
    });
}
_onPlayerReady(event);
{
    if (!this.isMounted()) {
        return;
    }
    this.player = event.target;
    this._maybeUpdatePlayer(this.props.videoID);
}
_onPlayerStateChange(event);
{
    // Respond to player events
    // Possible states:
    // {UNSTARTED: -1, ENDED: 0, PLAYING: 1, PAUSED: 2, BUFFERING: 3, CUED: 5}
    switch (event.data) {
        case YT.PlayerState.UNSTARTED:
            var videoID = event.target.getVideoData().video_id;
            if (videoID !== this.props.videoID) {
                this.props.onSwitchVideo(videoID);
            }
            break;
        case YT.PlayerState.PLAYING:
            this.props.onPlay && this.props.onPlay();
            break;
        case YT.PlayerState.PAUSED:
            this.props.onPause && this.props.onPause();
            break;
        case YT.PlayerState.ENDED:
            this.props.onEnd && this.props.onEnd();
            break;
        case YT.PlayerState.BUFFERING:
            this.props.onBuffering && this.props.onBuffering();
            break;
    }
}
;

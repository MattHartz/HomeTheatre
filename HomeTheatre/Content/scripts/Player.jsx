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

var VideoPlayer = React.createClass({
    propTypes: {
        videoId: React.PropTypes.string.isRequired,
        id: React.PropTypes.string,
        className: React.PropTypes.string,
        onReady: React.PropTypes.func,
        onError: React.PropTypes.func,
        onPlay: React.PropTypes.func,
        onPause: React.PropTypes.func,
        onEnd: React.PropTypes.func,
        onStateChange: React.PropTypes.func,
        onSwitchVideo: React.PropTypes.func,
        opts: React.PropTypes.object
    },
    /* Public API for controlling the player */

    play() {
        this.player && this.player.playVideo();
    },

    pause() {
        this.player && this.player.pauseVideo();
    },

    mute() {
        this.player && this.player.mute();
    },

    unMute() {
        this.player && this.player.unMute();
    },

    /* Life cycle methods */

    componentWillMount() {
        loadAPI(this._onAPIReady);
    },

    componentWillUnmount() {
        this.player && this.player.destroy();
    },

    componentWillReceiveProps(videoId) {
        this.player && this._maybeUpdatePlayer(videoId);
    },
    _maybeUpdatePlayer(videoId) {
        if (this.player.getVideoData().video_id !== videoId) {
            // If it's a different video => stop current video + load the new one!
            // (loading will automatically play the video)
            this.player.stopVideo();
            this.player.loadVideoById(videoId);
        }
    },
    _onPlayerReady(event) {
        if (!this.isMounted()) {
            return;
        }
        this.player = event.target;
        this._maybeUpdatePlayer(this.props.videoId);
    },
    _onAPIReady() {
        if (!this.isMounted()) {
            return;
        }

        new YT.Player('player', {
            videoId: this.props.videoId,
            playerVars: {
                controls: 0
            },
            events: {
                onReady: this._onPlayerReady,
                onStateChange: this._onPlayerStateChange,
            },
        });
    },
    _onPlayerReady(event) {
        if (!this.isMounted()) {
            return;
        }
        this.player = event.target;
        this._maybeUpdatePlayer(this.props.videoId);
    },
    _onPlayerStateChange(event) {
        // Respond to player events
        // Possible states:
        // {UNSTARTED: -1, ENDED: 0, PLAYING: 1, PAUSED: 2, BUFFERING: 3, CUED: 5}
        switch (event.data) {
            case YT.PlayerState.UNSTARTED:
                var videoId = event.target.getVideoData().video_id;
                if (videoId !== this.props.videoId) {
                    this.props.onSwitchVideo(videoId);
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
    },
    render: function () {
        var opts = {};
        var onReady = () => { };
        var onError = () => { };
        var onPlay = () => { };
        var onPause = () => { };
        var onEnd = () => { };
        var onStateChange = () => { };
        var onSwitchVideo = () => { };

        var style = { };

        return (
            <div className="videoPlayer">
                <div id="player"></div>
                <div className="buttonBar" style={style}>
                    <span className="glyphicon glyphicon-play" aria-hidden="true"></span>
                    <span className="glyphicon glyphicon-pause" aria-hidden="true"></span>
                    <span className="glyphicon glyphicon-stop" aria-hidden="true"></span>
                </div>
            </div>
        );
    }
});
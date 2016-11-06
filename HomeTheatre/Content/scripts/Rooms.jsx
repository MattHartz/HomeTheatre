// Rooms.jsx

var RoomWrapper = React.createClass({
    componentDidMount: function() {
        var chat = $.connection.roomHub;
        var room = window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1);

        $.connection.hub.start().done(function () {
            chat.server.JoinRoom(room);
        });
    },
    render: function () {
        return (
            <div className="roomWrapper row">
                <UsersBox />
                <ChatWrapper />
            </div>
        );
    }
});

var RoomTitle = React.createClass({
    getInitialState: function () {
        return { room: window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1) };
    },
    render: function () {
        return (
            <h2  className="roomTitle">Room {this.state.room}</h2>
        );
    }
});

var UsersBox = React.createClass({
    getInitialState: function () {
        return { users: [] };
    },
    componentDidMount: function () {
        var self = this;
        var chat = $.connection.roomHub;

        // users is a list of strings
        chat.client.broadcastUsers = function (users) {
            self.setState({
                users: users
            });
        };

        $.connection.hub.start();
    },
    render: function () {
        var userNodes = this.state.users.map(function (user) {
            return (
                <div key={user}>{user}</div>
            );
        });
        return (
            <div className="usersBox col-md-2">
                {userNodes}
            </div>
        );
    }
});

var Comment = React.createClass({
    render: function() {
        return (
          <div className="comment">
            <div className="commentAuthor">
                {this.props.author}
            </div>
            <div>
                {this.props.message}
            </div>
          </div>
      );
    }
});

var ChatLog = React.createClass({
    getInitialState: function () {
        return { comments: [] };
    },
    componentDidMount: function () {
        var self = this;
        var chat = $.connection.roomHub;

        chat.client.broadcastMessage = function (author, message) {
            var date = new Date().getTime();
            self.setState({
                comments: self.state.comments.concat({
                    author: author,
                    message: message,
                    time: date,
                    key: date + ' ' + author
                })
            });

            // Position scroll bar to bottom
            var node = ReactDOM.findDOMNode(self);
            node.scrollTop = node.scrollHeight + 20;
        };

        $.connection.hub.start();
    },
    render: function () {
        var commentNodes = this.state.comments.map(function (comment) {
            return (
                <Comment author={comment.author} message={comment.message} key={comment.key}>
                </Comment>
            );
        });
        return (
            <div className="chatLog">
                {commentNodes}
            </div>
        );
    }
});

var ChatWrapper = React.createClass({
    render: function () {
        return (
            <div className="chatWrapper col-md-10">
                <RoomTitle />
                <div className="chat">
                    <ChatLog />
                    <MessageWrapper />
                </div>
            </div>
        );
    }
});

var MessageWrapper = React.createClass({
    handleChange: function (event) {
        this.setState({ message: event.target.value });
    },
    onKeyDown: function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            this.onSubmit(event.target.value);
            event.target.value = "";
        }
    },
    onSubmit: function (message) {
        var chat = $.connection.roomHub;

        $.connection.hub.start().done(function () {
            chat.server.send(message);
        });
    },
    render: function () {
        return (
            <div className="messageWrapper">
                <textarea className="messageBox" 
                          onKeyDown={this.onKeyDown}
                          onChange={this.handleChange}>
                </textarea>
            </div>
        );
    }
});

ReactDOM.render(
    <RoomWrapper />,
    document.getElementById('chatContainer')
);
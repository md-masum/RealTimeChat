var realTimeChat = {}
var METHOD_SEND_DATA = "SendSignalRData";

realTimeChat.getSessionStorage = (key) => {
    return sessionStorage.getItem(key);
}

realTimeChat.setSessionStorage = (key, data) => {
    return sessionStorage.setItem(key, data);
}

realTimeChat.scrollToBottom = (id) => {
    var div = document.getElementById(id);
    if (div && div.scrollHeight && div.clientHeight) {
        div.scrollTop = div.scrollHeight - div.clientHeight;
    }
}

realTimeChat.playNotification = () => {
    document.getElementById('notification').play();
}

realTimeChat.playCall = () => {
    document.getElementById('call').play();
}

realTimeChat.pushCall = () => {
    document.getElementById('call').push();
}

// web RTC section
let username;
let localStream;
let peerConn;
let configuration = {
    iceServers: [
        {
            "urls": ["stun:stun.l.google.com:19302",
                "stun:stun1.l.google.com:19302",
                "stun:stun2.l.google.com:19302"]
        }
    ]
}

//trigger with signalR response
realTimeChat.handleSignallingData = (data, dotNetHelper) => {
    data = JSON.parse(data);
    switch (data.type) {
        case "answer":
            peerConn.setRemoteDescription(data.answer);
            break;
        case "candidate":
            peerConn.addIceCandidate(data.candidate);
        case "offer":
            peerConn.setRemoteDescription(data.offer);
            createAndSendAnswer(dotNetHelper);
            break;
    }
}

// have to call .net function to send signalR request
function sendData(data, dotNetHelper) {
    dotNetHelper.invokeMethodAsync(METHOD_SEND_DATA, JSON.stringify(data));
}

realTimeChat.startCall = (userName, dotNetHelper) => {
    sendData({
        type: "store_user",
        username: userName
    }, dotNetHelper);

    navigator.getUserMedia({
            video: {
                frameRate: 24,
                width: {
                    min: 480,
                    ideal: 720,
                    max: 1280
                },
                aspectRatio: 1.33333
            },
            audio: true
        },
        (stream) => {
            localStream = stream;
            document.getElementById("local-video").srcObject = localStream;

            peerConn = new window.RTCPeerConnection(configuration);
            peerConn.addStream(localStream);

            peerConn.onaddstream = (e) => {
                document.getElementById("remote-video")
                    .srcObject = e.stream;
            }

            peerConn.onicecandidate = ((e) => {
                if (e.candidate == null)
                    return;
                sendData({
                    type: "store_candidate",
                    candidate: e.candidate
                }, dotNetHelper);
            });

            createAndSendOffer(dotNetHelper);

            //joining call might be removed
            sendData({
                type: "join_call"
            }, dotNetHelper);
        },
        (error) => {
            console.log(error);
        });
}

function createAndSendOffer(dotNetHelper) {
    peerConn.createOffer((offer) => {
            sendData({
                type: "store_offer",
                offer: offer
            }, dotNetHelper);

            peerConn.setLocalDescription(offer);
        },
        (error) => {
            console.log(error);
        });
}

let isAudio = true;
function muteAudio() {
    isAudio = !isAudio;
    localStream.getAudioTracks()[0].enabled = isAudio;
}

let isVideo = true;
function muteVideo() {
    isVideo = !isVideo;
    localStream.getVideoTracks()[0].enabled = isVideo;
}

//Receiver Logic
function createAndSendAnswer(dotNetHelper) {
    peerConn.createAnswer((answer) => {
            peerConn.setLocalDescription(answer);
            sendData({
                type: "send_answer",
                answer: answer
            }, dotNetHelper);
        },
        error => {
            console.log(error);
        });
}
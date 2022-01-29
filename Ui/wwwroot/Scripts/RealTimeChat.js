var realTimeChat = {}

realTimeChat.getSessionStorage = (key) => {
    return sessionStorage.getItem(key);
}

realTimeChat.setSessionStorage = (key, data) => {
    return sessionStorage.setItem(key, data);
}
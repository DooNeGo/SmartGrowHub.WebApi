import http from 'k6/http';
import { sleep } from 'k6';
import * as config from './config.js';

export const options = {
    insecureSkipTLSVerify: false,
    noConnectionReuse: false,
    vus: 10,
    duration: '1m'
};

const registerRequest = {
    userName: "DooNeGo123",
    password: "qwerty123123",
    email: "asdasd@gmail.com",
    displayName: "DooNeGo"
}

const logInRequest = {
    userName: "DooNeGo123",
    password: "qwerty123123",
    email: "asdasd@gmail.com",
    displayName: "DooNeGo"
}

export default function () {
    http.post(config.API_LOGIN_URL, logInRequest);
}
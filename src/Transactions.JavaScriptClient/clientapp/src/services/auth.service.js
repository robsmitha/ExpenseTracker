import { BehaviorSubject } from 'rxjs';

const _appUserKey = 'appUser';

const appUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(_appUserKey)));

export const authService = {
    clearAppUser,
    setAppUser,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value },
    getToken
}

function setAppUser(appUser) {
    localStorage.setItem(_appUserKey, JSON.stringify(appUser));
    appUserSubject.next(appUser);
}

function clearAppUser() {
    // remove user from local storage to log user out
    localStorage.removeItem(_appUserKey);
    appUserSubject.next(null);
}

function getToken(){
    return appUserSubject.value != null ? appUserSubject.value.token : '';
}


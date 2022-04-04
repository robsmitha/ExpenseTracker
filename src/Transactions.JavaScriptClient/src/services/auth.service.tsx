
export const authService = {
    getAppUser,
    getToken,
    clearToken,
    setAppUser
}

function getAppUser(){
    const appUser = JSON.parse(localStorage.getItem("appUser") || '{}');
    return appUser;
}

function getToken(){
    const appUser = getAppUser();
    return appUser.token;
}

function clearToken(){
    localStorage.setItem("appUser", JSON.stringify({
        authenticated: false,
        token: null
    }));
}

function setAppUser(_: any){
    localStorage.setItem("appUser", JSON.stringify(_));
}
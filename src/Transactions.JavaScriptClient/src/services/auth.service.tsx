
export const authService = {
    getToken
}
function getToken(){
    const appUser = JSON.parse(localStorage.getItem("appUser") || '{}');
    return appUser.token;
}


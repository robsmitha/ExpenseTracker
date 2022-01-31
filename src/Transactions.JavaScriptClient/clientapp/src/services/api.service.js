
export async function sendRequest(func, request) {
    try {
        const response = await fetch(process.env.REACT_APP_API_ENDPOINT + func, request)
        if(response.ok){
            const data = await response.json()
            return data
        }
        else{
            switch(response.status){
                case 401: return 'Unauthorized api call'    //TODO: remove token
                default: return JSON.stringify(response)
            }
        }
    } catch (error) {
        return error.message
    }
}

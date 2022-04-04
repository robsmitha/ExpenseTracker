import { authService } from './auth.service'

export async function send(func: string, request: any) {
    const token = authService.getToken()
    try {
        if(token){
            if(!request?.headers){
                Object.assign(request, {
                    headers: {
                    
                    }
                });
            }
            Object.assign(request.headers, {
                'Authorization': `Bearer ${token}` 
            });
        }
        
        const response = await fetch(process.env.REACT_APP_API_ENDPOINT + func, request)
        if(response.ok){
            const data = await response.json()
            return data
        } else {
            switch(response.status){
                case 401: 
                    authService.clearToken();
                    window.location.href = '/';
                    return 'Unauthorized api call'
                default: 
                    return JSON.stringify(response)
            }
        }
    } catch (error: any) {
        return error.message
    }
}

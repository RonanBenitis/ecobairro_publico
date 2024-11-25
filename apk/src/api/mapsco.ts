import axios from "axios"

const mapscoApiKey = "671e5a59ec870931098681buy4c6cdf" // é de graça, cachorro kk
const mapscoBaseUrl = "https://geocode.maps.co"
export async function getAddress(lat: string, lon: string){
    return (await axios.get(`${mapscoBaseUrl}/reverse?lat=${lat}&lon=${lon}&api_key=${mapscoApiKey}`)).data
}
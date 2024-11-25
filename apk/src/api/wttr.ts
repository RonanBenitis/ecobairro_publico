import axios from "axios"

export async function getWeather(bairro: string){
    const data = (await axios.get(`https://wttr.in/${bairro}?format=1`)).data
    return data
}
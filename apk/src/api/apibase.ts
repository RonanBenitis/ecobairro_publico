import axios from "axios"

const baseurl = "https://ecobairro.onrender.com/api"

export async function getAllUsers(){
    return (await axios.get(`${baseurl}/users`)).data
}

export async function getAllPins(){
    return (await axios.get(`${baseurl}/pin`)).data
}

export async function getUserByEmail(email: string){
    return (await axios.get(`${baseurl}/user/email/${email}`)).data
}

export async function getUserById(id: any){
    return (await axios.get(`${baseurl}/user/${id}`)).data
}

export async function getAllFiscal(){
    return (await axios.get(`${baseurl}/fiscais`)).data
}

export async function getAllMunicipe(){
    return (await axios.get(`${baseurl}/municipes`)).data
}

export async function getFiscalByEmail(email: string){
    return (await axios.get(`${baseurl}/fiscal/email/${email}`)).data
}

export async function getMunicipeByEmail(email: string){
    return (await axios.get(`${baseurl}/municipes/email/${email}`)).data
}

export async function createPin(data: {
    municipeCriadorUserId: number,
    latitude: string, 
    longitude: string,
    endereco: string, 
    descricao: string 
 }) {
    console.log(data)
    return (await axios.post(`${baseurl}/pin`, data)).data
}

export async function aprovaPin(pinId: number, userId: number){
    return (await axios.put(`${baseurl}/pin/aprovapin/${pinId}?userId=${userId}`)).data
}

export async function reprovaPin(pinId: number){
    return (await axios.delete(`${baseurl}/pin/${pinId}`)).data
}

export async function setPoints( pinId: number, points: number, userId: number){
    return (await axios.put(`${baseurl}/pin/setpoints/${pinId}?points=${points}&userId=${userId}`)).data
}

export async function concluiPin(pinId: number, userId: number){
    return (await axios.put(`${baseurl}/pin/concluipin/${pinId}?userId=${userId}`)).data
}

export async function deletaPin(pinId: number){
    return (await axios.delete(`${baseurl}/pin/${pinId}`)).data
}

export async function getPoints(bairroId: number){
    return (await axios.get(`${baseurl}/bairro/getpoint/${bairroId}`)).data
}

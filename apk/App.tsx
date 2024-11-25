import { GluestackUIProvider } from "@/components/ui/gluestack-ui-provider";
import { Image, Text, View } from "react-native";
import "./global.css";

import {
  Table,
  TableBody,
  TableData,
  TableHead,
  TableHeader,
  TableRow
} from "@/components/ui/table";

import ModalComponent from "@/components/custom/ModalComponent";
import RecyclePin from "@/components/icons/RecyclePin";
import { Button, ButtonSpinner } from "@/components/ui/button";
import { ChevronDownIcon, EyeIcon, EyeOffIcon, MailIcon } from "@/components/ui/icon";
import { Input, InputField, InputIcon, InputSlot } from "@/components/ui/input";
import React, { useEffect, useState } from "react";
import MapView, { Circle, Marker, Region } from "react-native-maps";

import { Toast, useToast } from "@/components/ui/toast";

import Icon from "react-native-vector-icons/FontAwesome";


import { aprovaPin, concluiPin, createPin, deletaPin, getAllPins, getPoints, getUserByEmail, reprovaPin, setPoints } from "@/api/apibase";
import { getAddress } from "@/api/mapsco";
import { getWeather } from "@/api/wttr";
import { Box } from "@/components/ui/box";
import { HStack } from "@/components/ui/hstack";
import { Select, SelectBackdrop, SelectContent, SelectDragIndicator, SelectDragIndicatorWrapper, SelectIcon, SelectInput, SelectItem, SelectPortal, SelectTrigger } from "@/components/ui/select";
import { VStack } from "@/components/ui/vstack";
import { UserProvider, useUser } from "@/contexts/UserContext";
import { randrange } from "@/utils/numbers";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { signInWithEmailAndPassword } from "firebase/auth";
import { ScrollView } from "react-native";
import { auth } from './firebase';

const STOCKIMAGE = require('./assets/lixo.jpg')

function NewPin(props: {
  region: Region
  showPinModal: boolean
  setShowPinModal: any
}) {

  const { user } = useUser()
  const [inputData, setInputData] = useState({
    descricao: "",
  })
  const [loading, setLoading] = useState(false)
  const toast = useToast()

  async function submitPin() {
    setLoading(true)
    try {

      if (!inputData.descricao) {
        throw new Error()
      }

      const addressResponse = await getAddress(String(props.region.latitude), String(props.region.longitude))
      const address = `${addressResponse.address.road}, ${addressResponse.address.postcode}`

      await createPin({
        municipeCriadorUserId: user.backend.id,
        latitude: String(props.region.latitude),
        longitude: String(props.region.longitude),
        endereco: address,
        descricao: inputData.descricao
      })

      setLoading(false)
      props.setShowPinModal(false)
      renderToast(toast, "top", 3000, String("Pin enviado para análise."), "success")

    } catch (err) {
      setLoading(false)
      renderToast(toast, "top", 3000, String("Por favor, preencha os campos abaixo."), "error")
    }


  }

  return (
    <ModalComponent
      showModal={props.showPinModal}
      setShowModal={props.setShowPinModal}
      title="Solicitar coleta de lixo"
      content={
        user.backend.role === "Municipe" ?
          <View className="flex gap-2">
            <Input variant="outline" size="xl" className="w-full h-32 pr-3">
              <InputField multiline={true} className="py-2 align-top text-wrap" type="text" placeholder="Descrição" onChangeText={(e) => setInputData({ ...inputData, descricao: e })} value={inputData.descricao} />
            </Input>
          </View>
          :
          <View className="flex gap-2">
            <Text>Apenas munícipes podem criar pins.</Text>
          </View>


      }
      footer={
        user.backend.role === "Municipe" ?
          <Button disabled={loading} className="bg-green-700 p-3 h-fit flex-1" onPress={() => submitPin()}>
            <Text className="text-white">
              {loading ? <ButtonSpinner color={"white"} size={20} /> : "Cadastrar Pin"}
            </Text>
          </Button>
          : null
      }
    />
  )
}

function SelectedPin(props: { pinData: any, showPinModal: boolean, setShowPinModal: any, reload?: any }) {

  const { user } = useUser()
  const toast = useToast()
  const [loading, setLoading] = useState({
    concludePin: false,
    deletePin: false
  })

  async function handleConcludePin() {
    setLoading({ ...loading, concludePin: true });

    try {
      // FIXME atribui pontuação arbitrária ao pin
      await setPoints(props.pinData.id, randrange(100, 500), user.backend.id);
      await concluiPin(props.pinData.id, user.backend.id);
      renderToast(toast, "top", 3000, String("Pin concluído!"), "success");
    } catch (err) {
      renderToast(toast, "top", 3000, String("Erro ao concluir pin"), "error");
    }


    setLoading({ ...loading, concludePin: false });
    props.reload.set(!props.reload.state)
    props.setShowPinModal(false);
  }

  async function handleDeletePin() {
    setLoading({ ...loading, deletePin: true })

    try {
      await deletaPin(props.pinData.id);
      renderToast(toast, "top", 3000, String("Pin excluído"), "success");
    } catch (err) {
      renderToast(toast, "top", 3000, String("Erro ao deletar pin"), "error");
    }

    setLoading({ ...loading, deletePin: false })
    props.reload.set(!props.reload.state)
    props.setShowPinModal(false);
  }

  return (
    <ModalComponent
      showModal={props.showPinModal}
      setShowModal={props.setShowPinModal}
      title={
        <Text>{props.pinData.categoria}</Text>
      }

      content={
        <View className="flex-1 justify-center items-center gap-5">
          <View className="w-full gap-2">
            <Image className="w-full h-56 rounded" source={STOCKIMAGE} />
            <Text className="font-bold">Solicitante: {props.pinData.nomeMunicipe}</Text>
            <Text className="font-bold">Aprovador: {props.pinData.nomeFiscal}</Text>
          </View>



          <Box className="rounded-lg overflow-hidden border border-typography-700 w-full">
            <Table className="w-full">
              <TableRow>
                <TableData className="text-md">Descrição</TableData>
                <TableData className="text-md">{props.pinData.descricao}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Endereço</TableData>
                <TableData className="text-md">{props.pinData.endereco}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Latitude</TableData>
                <TableData className="text-md">{String(props.pinData.latitude).slice(0, 9)}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Longitude</TableData>
                <TableData className="text-md">{String(props.pinData.longitude).slice(0, 9)}</TableData>
              </TableRow>
            </Table>
          </Box>

        </View>
      }
      footer={

        user.backend.role === "Fiscal" ?
          <View className="flex flex-row w-full justify-between gap-2">
            <Button
              disabled={loading.concludePin}
              className="bg-success-500 w-2/3"
              onPress={() => {
                handleConcludePin()

              }}
            >
              <Text className="text-white">
                {loading.concludePin ? <ButtonSpinner color={"white"} size={20} /> : "Concluir Pin"}
              </Text>
            </Button>


            <Button
              disabled={loading.deletePin}
              className="bg-error-500 w-1/3"
              onPress={() => {
                handleDeletePin()

              }}
            >
              <Text className="text-white">
                {loading.deletePin ? <ButtonSpinner color={"white"} size={20} /> : "Excluir Pin"}
              </Text>
            </Button>
          </View>

          :

          <View className="flex-1 bg-info-500 p-3 items-center gap-4 rounded">
            <Text className="text-center text-xl text-white">Em andamento</Text>
          </View>
      }
    />
  )
}

function PinAnalysis(props: { pinData: any, reload: any, user: any, toast: any }) {
  const [showApprovePinModal, setShowApprovePinModal] = useState(false);
  const [loading, setLoading] = useState({
    approvePin: false,
    rejectPin: false,
  })

  async function handleApprovePin(pin: any, userId: number) {

    setLoading({
      ...loading,
      approvePin: true,
    })

    try {
      await aprovaPin(pin.id, userId)
      renderToast(props.toast, "top", 3000, String("Pin aprovado"), "success")
      setShowApprovePinModal(false)

    } catch (err) {
      renderToast(props.toast, "top", 3000, String("Erro ao aprovar pin"), "error")
      throw err
    }
    props.reload.set(!props.reload.state)

    setLoading({
      ...loading,
      approvePin: false,
    })
  }

  async function handleRejectPin(pin: any) {
    setLoading({
      ...loading,
      rejectPin: true,
    })

    try {
      await reprovaPin(pin.id)
      renderToast(props.toast, "top", 3000, String("Pin reprovado"), "success")
      setShowApprovePinModal(false)

    } catch (err) {
      renderToast(props.toast, "top", 3000, String("Por favor, tente novamente mais tarde"), "error")
    }
    props.reload.set(!props.reload.state)

    setLoading({
      ...loading,
      rejectPin: false,
    })

  }

  return (
    <ModalComponent
      key={`${props.pinData.id}-approveModal`}
      showModal={showApprovePinModal}
      setShowModal={setShowApprovePinModal}
      buttonstyle="bg-green-700"
      title="Analisar Pin"
      openbutton={
        <Text className="text-white">Analisar</Text>
      }
      content={
        <View className="flex-1 justify-center items-center gap-5">
          <View className="w-full gap-2">
            <Image className="w-full h-56 rounded" source={STOCKIMAGE} />
            <Text className="font-bold">Solicitante: {props.pinData.nomeMunicipe}</Text>
          </View>

          <Box className="rounded-lg overflow-hidden border border-typography-700 w-full">
            <Table className="w-full">
              <TableRow>
                <TableData className="text-md">Descrição</TableData>
                <TableData className="text-md">{props.pinData.descricao}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Endereço</TableData>
                <TableData className="text-md">{props.pinData.endereco}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Latitude</TableData>
                <TableData className="text-md">{String(props.pinData.latitude).slice(0, 9)}</TableData>
              </TableRow>
              <TableRow>
                <TableData className="text-md">Longitude</TableData>
                <TableData className="text-md">{String(props.pinData.longitude).slice(0, 9)}</TableData>
              </TableRow>
            </Table>
          </Box>

        </View>
      }

      footer={
        <HStack className="flex-1 gap-2">
          <Button
            disabled={loading.approvePin}
            className="bg-success-500 flex-1"
            onPress={() => {
              handleApprovePin(props.pinData, props.user.backend.id)
            }}
          >
            <Text className="text-white">
              {loading.approvePin ? <ButtonSpinner color={"white"} size={20} /> : "Aprovar"}
            </Text>
          </Button>
          <Button
            disabled={loading.rejectPin}
            className="bg-error-500 flex-1"
            onPress={() => {
              handleRejectPin(props.pinData)
            }}
          >
            <Text className="text-white">
              {loading.rejectPin ? <ButtonSpinner color={"white"} size={20} /> : "Reprovar"}
            </Text>
          </Button>
        </HStack>
      }

    />
  )
}

function Settings(props: { userContext: any, reload: any, config?: any }) {
  const [showSettingsModal, setShowSettingsModal] = useState(false)

  function handleMapStyleChange(itemValue: string) {
    props.config?.setMapStyle(itemValue)
  }


  async function logOut() {
    try {
      await auth.signOut();
      props.userContext.setUser(null);
      await AsyncStorage.removeItem('user');
      setShowSettingsModal(false)
    } catch (err) {
      console.log({ err });
    }
  }

  return (
    <ModalComponent
      showModal={showSettingsModal}
      setShowModal={setShowSettingsModal}
      buttonstyle="bg-green-700"
      title={`Olá, ${props.userContext.user.backend.username}!`}
      openbutton={
        <Text className="text-white">
          <Icon name="user" />
        </Text>
      }
      content={
        <View className="flex flex-col gap-5">
          <Box className="flex flex-row items-center gap-5">
            <Table className="w-full border rounded-md border-gray-300">
              <TableHeader>
                <TableRow>
                  <TableHead className="text-lg text-center">Configurações</TableHead>
                </TableRow>
              </TableHeader>
              <TableRow className="flex items-center">
                <View className="flex flex-row w-full items-center justify-between px-5 py-2">
                  <Text className="text-gray-700 text-md">Estilo do mapa</Text>
                  <Select onValueChange={(itemValue) => handleMapStyleChange(itemValue)}>
                    <SelectTrigger variant="outline">
                      <SelectInput className="" placeholder="Selecionar" />
                      <SelectIcon className="mr-3" as={ChevronDownIcon} />
                    </SelectTrigger>
                    <SelectPortal>
                      <SelectBackdrop />
                      <SelectContent>
                        <SelectDragIndicatorWrapper>
                          <SelectDragIndicator />
                        </SelectDragIndicatorWrapper>
                        <SelectItem label="Hybrid" value="hybrid" />
                        <SelectItem label="Satellite" value="satellite" />
                        <SelectItem label="Standard" value="standard" />
                        <SelectItem label="Terrain" value="terrain" />
                      </SelectContent>
                    </SelectPortal>
                  </Select>
                </View>
              </TableRow>
            </Table>
          </Box>

        </View>
      }
      footer={
        <Button
          className="bg-green-700 w-full"
          onPress={logOut}
        >
          <Text className="text-white">Sair</Text>
        </Button>
      }
    />)
}

function renderToast(toast: any, placement: string, duration: number, message: string, status?: string) {
  toast.show({
    placement: placement,
    duration: duration,
    render: () => {
      return (
        <Toast className={`top-16 ${status ? `bg-${status}-500` : "bg-white"} rounded`}>
          <Text className={`${status ? "text-white" : "text-black"} text-xl px-4"`}>{message}</Text>
        </Toast>
      )
    }
  })
}


function Login() {

  const toast = useToast()
  const { user, setUser } = useUser();

  const [inputData, setInputData] = useState<{ email?: string, password?: string }>({});
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState({
    signIn: false,
    fetchUser: true,
  });


  async function saveAuthState(user: any) {
    try {
      await AsyncStorage.setItem('user', JSON.stringify(user));
    } catch (error) {
      console.error('Failed to save user data', error);
    }
  }

  async function loadAuthState() {
    try {
      const user = await AsyncStorage.getItem('user');
      if (user !== null) {
        setUser(JSON.parse(user));
      }
    } catch (error) {
      console.error('Failed to load user data', error);
    }

    setLoading({ ...loading, fetchUser: false })
  }

  async function signIn() {
    setLoading({ ...loading, signIn: true })

    try {
      const firebaseUser = await signInWithEmailAndPassword(auth, inputData.email!, inputData.password!);
      if (firebaseUser) {
        const backendUser = await getUserByEmail(inputData.email!);
        const user = {
          "firebase": firebaseUser,
          "backend": backendUser
        }
        setUser(user);
        saveAuthState(user);
      }

    } catch (err) {
      renderToast(toast, "top", 3000, String("E-mail ou senha incorretos"), "error");
    }

    setLoading({ ...loading, signIn: false })
  }

  useEffect(() => {
    loadAuthState();
  }, [])

  return (
    <>
      {
        loading.fetchUser ? <ButtonSpinner size={100} color={"green"} className="absolute top-0 left-0 right-0 bottom-0 z-50" />
          :
          <View className="flex-col justify-center gap-10 items-center">
            <View className="flex-row gap-5">
              <Icon name="recycle" size={100} color="green" />
              <Text className="text-[52px] text-typography-500">EcoBairro</Text>
            </View>
            <View className="flex gap-10">
              {/* Email */}
              <View className="flex gap-2">
                <Input variant="outline" size="xl" className="w-96 pr-3">
                  <InputField placeholder="Email" onChangeText={(e) => setInputData({ ...inputData, email: e })} value={inputData.email} />
                  <InputSlot>
                    <InputIcon as={MailIcon} />
                  </InputSlot>
                </Input>
                {/* Password */}
                <Input variant="outline" size="xl" className="w-96 pr-3">
                  <InputField type={showPassword ? "text" : "password"} placeholder="Senha" onChangeText={(e) => setInputData({ ...inputData, password: e })} />
                  <InputSlot onPress={() => { setShowPassword(!showPassword) }}>
                    <InputIcon as={showPassword ? EyeIcon : EyeOffIcon} />
                  </InputSlot>
                </Input>
              </View>
              <Button disabled={loading.signIn} className="bg-green-700 p-5 h-fit" onPress={signIn}>
                <Text className="text-white text-xl">
                  {loading.signIn ? <ButtonSpinner color={"white"} size={20} /> : "Entrar"}
                </Text>
              </Button>

            </View>
          </View>
      }


    </>
  )
}

function Home() {

  const { user, setUser } = useUser();
  const toast = useToast();

  const [showNewPinModal, setShowNewPinModal] = useState(false)
  const [showAdminModal, setShowAdminModal] = useState(false)
  const [showSelectedPinModal, setShowSelectedPinModal] = useState(false)
  const [selectedPinData, setSelectedPinData] = useState({})
  const [weather, setWeather] = useState("")
  const [pins, setPins] = useState<any[]>([])
  const [reload, setReload] = useState(false)
  const [bairroPoints, setBairroPoints] = useState(0)
  const [mapStyle, setMapStyle] = useState<any>("terrain")

  useEffect(() => {

    // get bairro points
    getPoints(1).then((res) => {
      setBairroPoints(res)
    })

    // get weather
    getWeather("Jundiapeba").then((res) => {
      setWeather(res)
    })

    // get pins
    getAllPins().then((res) => {
      setPins(res)

    })

  }, [reload, user])


  const [region, setRegion] = useState<Region>({
    latitude: -23.548965,
    longitude: -46.253075,
    latitudeDelta: 0.03,
    longitudeDelta: 0.03,
  })


  return (
    <View className="flex-1 justify-center items-center">
      {user ?
        <>
          <NewPin
            region={region}
            showPinModal={showNewPinModal}
            setShowPinModal={setShowNewPinModal}
          />
          <SelectedPin
            pinData={selectedPinData}
            showPinModal={showSelectedPinModal}
            setShowPinModal={setShowSelectedPinModal}
            reload={{ state: reload, set: setReload }}
          />
          <MapView
            style={{ width: "100%", height: "100%" }}
            initialRegion={region}
            mapType={mapStyle}
            onPress={(e) => {
              console.log(e.nativeEvent.coordinate)
              setRegion({ ...region, ...e.nativeEvent.coordinate })
              setShowNewPinModal(true)
            }}
          >
            {pins.map((pin: any) => {
              if (pin.status === "Aprovado") {
                return (
                  <View key={`${pin.id}-view`}>
                    <Marker
                      key={`${pin.id}-marker`}
                      coordinate={{
                        latitude: Number(pin.latitude),
                        longitude: Number(pin.longitude),
                      }}
                      onPress={() => {
                        setSelectedPinData(pin)
                        setShowSelectedPinModal(true)
                      }}
                    >
                      <View className="w-[40px] h-[50px]">
                        <RecyclePin />
                      </View>

                    </Marker>
                    <Circle
                      key={`${pin.id}-circle`}
                      center={{
                        latitude: Number(pin.latitude),
                        longitude: Number(pin.longitude),

                      }}
                      radius={50}
                      fillColor="#2B7B5755"
                      strokeColor="#2B7B5755"
                    />
                  </View>
                )
              }
            })}
          </MapView>

          {/* ========= HEADER ========= */}
          <View className="absolute p-3 top-0 h-30 w-full bg-white bg-opacity-10 flex-col justify-between items-center">
            <View className="mt-8 w-full flex-row justify-between items-center">
              <HStack className="gap-3 items-center">
                <Icon name="recycle" size={32} color="green" />
                <Text className="text-2xl text-typography-700">EcoBairro</Text>
              </HStack>

              <HStack className="gap-3">
                <Button onPress={() => setReload(!reload)} className="bg-green-700">
                  <Icon name="refresh" color="white"></Icon>
                </Button>

                {user.backend.role === "Fiscal" ?
                  <Text className="bg-red-500 h-6 rounded-full px-2 text-white text-s absolute right-[85px] top-[-5px] z-10">
                    {pins.filter((pin) => pin.status === "Aguardando análise").length}
                  </Text>
                  : null
                }
                {user.backend.role === "Fiscal" ?
                  <ModalComponent
                    showModal={showAdminModal}
                    setShowModal={setShowAdminModal}
                    buttonstyle="bg-green-700"
                    title="Painel Fiscal"
                    openbutton={
                      <Text className="text-white">
                        <Icon name="envelope" />
                      </Text>
                    }
                    content={
                      pins.some(pin => pin.status === "Aguardando análise") ?
                        <VStack className="mt-3 flex flex-col gap-0 max-h-96">
                          <Table className="w-full border rounded border-gray-300 border-b-0">
                            <TableHeader>
                              <TableRow className="w-full">
                                <TableHead className="text-md text-start">Munícipe</TableHead>
                                <TableHead className="border-r border-l border-gray-300 text-md text-start">Categoria</TableHead>
                                <TableHead className="text-md text-center ">Ação</TableHead>
                              </TableRow>
                            </TableHeader>
                            <TableBody>
                              <ScrollView>
                                {pins.map((pin) => {
                                  if (pin.status === "Aguardando análise") {
                                    return (
                                      <TableRow key={`${pin.id}-row`} className="w-full items-center justify-center">
                                        <TableData className="text-gray-700 text-start self-center text-md">{pin.nomeMunicipe}</TableData>
                                        <TableData className="border-r border-l border-gray-300 text-gray-700 text-start self-center text-md">{pin.categoria}</TableData>
                                        <TableData className="text-gray-700 text-center self-center text-md">
                                          <PinAnalysis
                                            key={`${pin.id}-admin`}
                                            toast={toast}
                                            pinData={pin}
                                            reload={{ state: reload, set: setReload }}
                                            user={user}
                                          />
                                        </TableData>
                                      </TableRow>
                                    )
                                  }
                                })}
                              </ScrollView>                              
                            </TableBody>
                          </Table>                          
                        </VStack>
                        : <Text>Não há pins para aprovar.</Text>

                    }
                  />
                  : null
                }

                <Settings
                  userContext={{ user: user, setUser: setUser }}
                  reload={{ state: reload, set: setReload }}
                  config={{
                    mapStyle: mapStyle, setMapStyle: setMapStyle
                  }}
                />
              </HStack>

            </View>
            <View className="mt-2 w-full flex-row justify-between items-center">
              <View className="flex-row items-center gap-2">
                <Text className="text-start text-xl text-typography-700">Jundiapeba: {bairroPoints}</Text>
                <Icon name="star" size={20} color="orange" />
              </View>
              <Text className="text-xl  text-typography-700">{String(weather).trim()}</Text>
            </View>
          </View>

        </>
        :
        <Login />
      }
    </View >
  );
}

export default function App() {

  return (
    <GluestackUIProvider mode="light">
      <UserProvider>

        <Home />

      </UserProvider>
    </GluestackUIProvider>
  )
}






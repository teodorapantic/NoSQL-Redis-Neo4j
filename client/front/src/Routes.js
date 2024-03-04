import {Routes, Route, BrowserRouter} from 'react-router-dom';
import Logovanje from './komponente/Logovanje/Logovanje';
import Navigacija from './komponente/Navigacija/Navigacija';
import React, { useEffect, useState } from 'react';
import Registracija from './komponente/Registracija/Registracija';
import Kategorija from './komponente/Kategorija/Kategorija';
import Pocetna from './Pocetna'
import SamoProizvod from './komponente/SamoProizvod/SamoProizod';
import Dostavljac from './Dostavljac';
import Pretraga from './komponente/Pretraga';
import {useRef} from 'react';
import *  as signalR from '@microsoft/signalr';
import { MDBBtn, MDBAlert } from 'mdb-react-ui-kit';

import Akcije from './komponente/Akcije/Akcije';
function Rute ()
{
    const [showAlert, setShowAlert] = useState(false);
    const[message, setMessage]= useState(false);

    let user;
    if(localStorage.getItem('user-info'))
    {
        user = JSON.parse(localStorage.getItem('user-info'))
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:44332/producthub")
        .build();
    
        connection.on("ProductNotification" +user.userName , (productId) => {
            let obj = JSON.parse(productId);
            console.log(obj);
            setMessage(obj);
            setShowAlert(true)
            });
        connection.start();
    }
    
    return (<div style={{ width:'100%', height:'100vh' }}> 
       
        <Navigacija />
        {showAlert && (
        <div className="alert alert-primary" role="alert">
          {"Market :" +message.Market+
          ", porucuje: "+ message.Text}
        </div>
        )}
        <div >
        <BrowserRouter>
        <Routes>  
       
            <Route path='/logovanje' element={<Logovanje />} />
            <Route path='/registracija' element={<Registracija/>} />
            <Route path='/kategorija/:name/:IDCat' element={<Kategorija/>} />
            <Route path='/' element={<Pocetna/>} />
            <Route path='/proizvod/:IdProduct' element={< SamoProizvod />} />
            <Route path='dostava' element={<Dostavljac/>} />
            <Route path='/pretraga' element = {<Pretraga/>} />
            <Route path='/market/:name/:IDMarket' element={<Akcije/>} />
        </Routes>
        </BrowserRouter>
        </div>

        </div>
           );
}
export default Rute;
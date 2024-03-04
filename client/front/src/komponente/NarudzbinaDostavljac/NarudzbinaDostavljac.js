import React, {useState, useEffect} from 'react';
import *  as signalR from '@microsoft/signalr';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBCardHeader,
  MDBCardFooter,
  MDBBtn,
  MDBCol,
  MDBRow,
  MDBContainer
} from 'mdb-react-ui-kit';



function NarudzbinaDostavljac()
{
    const[message , setMessage] =useState(false);
    let messages =[];
          let deliver = localStorage.getItem("delivery-info");
          console.log(deliver);
          const connection = new signalR.HubConnectionBuilder()
          .withUrl("https://localhost:44332/producthub")
          .build();
          
          connection.on("DeliveryNotification" +deliver , (productId) => {
                let obj = JSON.parse(productId);
                console.log(obj);
                setMessage(obj);
                messages.push(obj);
                });
          connection.start();

    const data = [
        {
          Market: "Cutura",
          Proizvod: "Banane",
          Kolicina: "3kg",
          Cena: "600din",
          Adresa: "Cara Lazara",
          BrojTelefona: "0611174598",
          ImeMusterije: "Marko"
        },
        {
          Market: "Lidl",
          Proizvod: "Jabuke",
          Kolicina: "2kg",
          Cena: "450din",
          Adresa: "Bulevar kralja Aleksandra",
          BrojTelefona: "0622334455",
          ImeMusterije: "Petar"
        }
      ];


      return ( <div>{message ? (<><div><MDBRow>
        { 
          <MDBCol >
            <MDBCard className='text-center mb-3'>
                 <MDBCardHeader><MDBCardTitle style={{ fontSize: '25px' }}>Market: {message.MarketName}</MDBCardTitle></MDBCardHeader>
                 <MDBCardBody>
                   {/* <MDBCardTitle>Proizvod: {nar.Proizvod}</MDBCardTitle>
                   <MDBCardTitle>Količina: {nar.Kolicina}</MDBCardTitle>
                   <MDBCardTitle>Cena: {nar.Cena}</MDBCardTitle>
                  <MDBCardTitle>Adresa: {nar.Adresa}</MDBCardTitle>
                   <MDBCardTitle>Ime: {nar.ImeMusterije}</MDBCardTitle>
                   <MDBCardTitle>BrojTelefona: {nar.BrojTelefona}</MDBCardTitle> */}
      
                   <p style={{ margin: 0 }}><b>Proizvod:</b> {message.ProductName}</p>
                   <p style={{ margin: 0 }}><b> Količina:</b> {message.Quantity}</p>
                   <p style={{ margin: 0 }}><b> Cena:</b> {message.Price}</p>
                   <p style={{ margin: 0 }}><b>Adresa:</b> {message.Location}</p>
                   <p style={{ margin: 0 }}><b>Ime: </b>{message.UserName}</p>
                   <p style={{ margin: 0 }}><b>Broj:</b> {message.PhoneNumber}</p>
                  
                 </MDBCardBody>
                 <MDBCardFooter className='text-muted'></MDBCardFooter>
               </MDBCard>
          </MDBCol>
        }
      </MDBRow></div></>):(<><div>Cekamo Narudzbine</div></>)} </div>


          
           );
     
           
     
}
     export default NarudzbinaDostavljac;


import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Logovanje from '../Logovanje/Logovanje';

import { useState, useEffect } from "react";
import axios from "axios";
import Notifikacije from '../Notifikacije/Notifikacije';
import './Navigacija.css'
import * as signalR from "@microsoft/signalr";





import {
  MDBContainer,
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarToggler,
  MDBIcon,
  MDBNavbarNav,
  MDBNavbarItem,
  MDBNavbarLink,
  MDBBtn,
  MDBDropdown,
  MDBDropdownToggle,
  MDBDropdownMenu,
  MDBDropdownItem,
  MDBCollapse,
  MDBBadge,
  MDBPopover,
  MDBPopoverBody,
  MDBPopoverHeader,
 
} from 'mdb-react-ui-kit';
import { Button } from 'react-bootstrap';

const Navigacija = (props) =>
{
  const [popoverOpen, setPopoverOpen] = useState(false);
  const[login, setlogin]= useState("");
  const[notifications, setNotifications]= useState(false);
  const [categories, setCategories] = useState();
  const [markets, setMarkets] = useState();
  const[user_info, setUserinfo]=useState("");
  const[delivery_info, setDeliveryinfo]=useState("");
  const[search, setSearch] = useState("");
  const[param, setparam] = useState("")
  const user = JSON.parse(localStorage.getItem('user-info'));
  const del=localStorage.getItem('delivery-info');
  const [message, setMessage] = useState('');
  
 
  if(user){
  const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:44332/producthub")
  .build();

  console.log(user.userName);
  
  connection.on("ProductNotification", (productId) => {
     
   
      let obj = JSON.parse(productId);
      console.log(obj);
      setMessage(obj);
     
    

  });

   connection.start();
  }
  


  async function searchh()
  {
    let search = await fetch("https://localhost:44332/Product/SearchProducts/"+param ,
    {
      method: 'GET',
        headers: {

            "Content-type": "application/json;charset=UTF-8",

        }
    } ).then(Response=>{
      return Response.json();
    }).then((data)=>{
      const pro ={
        ...data[0],
      }
      console.log(pro);
      setSearch(pro);
    });
  }

  console.log(user);
  useEffect(()=>{
   
    
    console.log(user);
    if (user!=null)
     setUserinfo(user);
    // console.log(user_info);
    //console.log(del);

    if (del!=null)
      setDeliveryinfo(del);


    axios.get("https://localhost:44332/GetAllCategories")
    .then(res => {
      //console.log(res)
      setCategories(res.data)
    })
    .catch(err => {
      console.log(err)
    })

    axios.get("https://localhost:44332/GetAllMarkets")
    .then(res => {
      //console.log(res)
      setMarkets(res.data)
    })
    .catch(err => {
      console.log(err)
    })

    
  },[]
  )
  
  
  function notificationsShow()
  {
    setNotifications(true);
  }
  function notificationsHide()
  {
    setNotifications(false);
  }

  

  function loginshow() {
    setlogin(true);
  }
  function loginhide() {
    setlogin(false);
  }

  function logout()
    {
      if(localStorage.getItem('user-info')) 
       // localStorage.removeItem('user-info');
       localStorage.clear();
      //history.push("/");
      else 
        localStorage.removeItem('delivery-info');
      //window.location.reload();
      window.location.href='/';
    }
    
    
    
    
    
     
    
 

  return (
    

       

    <MDBNavbar expand='lg'  bgColor='light'>
      <MDBContainer fluid>
       

        { delivery_info ? (<><MDBNavbarBrand > {delivery_info}  </MDBNavbarBrand></>): (<> <MDBNavbarBrand ><img src="https://cdn-icons-png.flaticon.com/512/2156/2156021.png" style={{ height: '30px', objectFit: 'cover' }} ></img></MDBNavbarBrand></>)}
         

        <MDBNavbarToggler
          aria-controls='navbarSupportedContent'
          aria-expanded='false'
          aria-label='Toggle navigation'


        >
          <MDBIcon icon='bars' fas />
        </MDBNavbarToggler>


        
        <MDBCollapse navbar >
        <MDBNavbarNav className='mr-auto mb-2 mb-lg-0'>


        {delivery_info ? (<>
          </>)
          : (<>
          <MDBNavbarItem>
              <MDBNavbarLink active aria-current='page' href='/'>
                Početna
              </MDBNavbarLink>
            </MDBNavbarItem>


            <MDBNavbarItem>
              <MDBDropdown>
                <MDBDropdownToggle tag='a' className='nav-link' role='button'>
                  Kategorije
                </MDBDropdownToggle>
                <MDBDropdownMenu>
                  {categories ? categories.map(category => (
                    // <MDBDropdownItem key={category.tempID} >  <a href={`/kategorija/${category.name}/${category.tempID}`} style={{ color: '#393f81' }}>
                    <MDBDropdownItem key={category.tempID} link >  <a href={`/kategorija/${category.name}/${category.tempID}`} style={{ color: '#393f81' }}>
                      {category.name}
                    </a></MDBDropdownItem>
                  )) : <p>Loading...</p>}
                </MDBDropdownMenu>
              </MDBDropdown>
            </MDBNavbarItem>

            <MDBNavbarItem>
              <MDBDropdown>
                <MDBDropdownToggle tag='a' className='nav-link' role='button'>
                  Akcije
                </MDBDropdownToggle>
                <MDBDropdownMenu>
                {markets ? markets.map(market => (
                    // <MDBDropdownItem key={category.tempID} >  <a href={`/kategorija/${category.name}/${category.tempID}`} style={{ color: '#393f81' }}>
                    <MDBDropdownItem key={market.id} link >  <a href={`/market/${market.name}/${market.id}`} style={{ color: '#393f81' }}>
                      {market.name}
                    </a></MDBDropdownItem>
                  )) : <p>Loading...</p>}
                </MDBDropdownMenu>
              </MDBDropdown>
            </MDBNavbarItem></>)}
          
            


          </MDBNavbarNav>

          
          
          {delivery_info ? (<></>):
          (<><form className='d-flex input-group w-auto'>
            <input type='search' className='form-control' placeholder='Pretraži' aria-label='Search' onChange={(e)=>{setparam(e.target.value)}} />
            <Button color='primary'><MDBIcon fas icon="search" onClick={searchh} /></Button>
          </form></>)}

          

          


          <Logovanje show={login} onHide={loginhide}></Logovanje>
          
          {/* <MDBNavbarItem> <MDBNavbarLink onClick={notificationsShow} eventkey={2}>Notifikacije </MDBNavbarLink></MDBNavbarItem>
          <Notifikacije show={notifications} onHide={notificationsHide}></Notifikacije> */}
          

          {/* <MDBPopover color='secondary' btnChildren='Popover on bottom' placement='bottom' onClick={notificationsShow}></MDBPopover>
          <Notifikacije show={notifications} onHide={notificationsHide}></Notifikacije> */}


      {user_info ? (<> 
          
       

          {/* <div>   
            {setNotifications && <Notifikacije {...user_info} onClose={() => setNotifications(false)}  />}
          </div> */}

         
        </>  ) : (<>
          </>)
      }

          
    {user_info || delivery_info ? (<>        
            <MDBNavbarLink onClick={logout} eventkey={2} style={{ whiteSpace: 'nowrap' }}>Odjavi se</MDBNavbarLink>
       
        </>  ) : (<>
        
            <MDBNavbarLink onClick={loginshow} eventkey={2} style={{ whiteSpace: 'nowrap' }}>Prijavi se</MDBNavbarLink>
            <MDBNavbarLink href="/registracija" eventkey={2} style={{ whiteSpace: 'nowrap' }}>Registruj se</MDBNavbarLink>

          </>)
    }

   



         

        </MDBCollapse>
      </MDBContainer>
    </MDBNavbar>

  
  );
}
export default Navigacija;
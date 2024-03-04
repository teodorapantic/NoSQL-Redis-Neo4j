

import React from "react";
import {useState} from "react";
import { MDBListGroup, MDBListGroupItem } from 'mdb-react-ui-kit';
import {

  MDBPopover,
  MDBPopoverBody,
  MDBPopoverHeader
  
} from "mdb-react-ui-kit";

import './Notifikacije.css'


function Notifikacije(props){
  const[notifications, setNotifications]= useState("");
  const[marketi, setMarketi]= useState("");
  
    const { onClose } = props;
    // console.log(props);
    // console.log(props.returnID);

    //notifikacije();
    async function notifikacije()
    {



    let result =  await fetch("https://localhost:44332/Notification/GetUserNotification/"+props.returnID,
    {method: 'GET',
    headers: {

        "Content-type": "application/json;charset=UTF-8",

    }});

   
    let data = await result.json();
    console.log(data);
    setNotifications(data.map(p => p.text));
    setMarketi(data.map(p => p.market))
    console.log(notifications);
  }
  notifikacije();

  console.log(notifications);
   

              
    return (
        
            <MDBPopover color='light' btnChildren={<><a  className=" mx-3">
            <i className="fas fa-envelope fa-2x"></i>
            
          </a> </>}> 
                    <MDBPopoverHeader color='light' >Notifikacije</MDBPopoverHeader>
                    <MDBPopoverBody color='secondary'>
                      <MDBListGroup>
                        {notifications ? notifications.map((notification,i) => (
                    // <MDBDropdownItem key={category.tempID} >  <a href={`/kategorija/${category.name}/${category.tempID}`} style={{ color: '#393f81' }}>
                    
                    <MDBListGroupItem noBorders color='light' className='px-3 mb-2 rounded-3'  >
                       <span style={{ fontWeight: 'bold' }}>{marketi[i]}:</span> {notification}
                    </MDBListGroupItem>
                  )) : <p>Zapratite proizvod da biste dobili obaveštenja kada su na akciji.</p>}
                      </MDBListGroup>
                    </MDBPopoverBody>
                  </MDBPopover>
                  

    );   
}
export default Notifikacije;

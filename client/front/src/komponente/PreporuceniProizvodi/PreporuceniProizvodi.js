
import React from "react";
import { useEffect, useState } from "react";
import { Col,  Card } from "react-bootstrap";

import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCardTitle,
  MDBCardText,
  MDBIcon,
  MDBRipple,
  MDBBtn,
} from "mdb-react-ui-kit";
import axios from "axios";



function PreporuceniProizvodi() {


const [products, setProducts] = useState([]);
 

  useEffect(() => {
    axios.get(`https://localhost:44332/User/GetRecommendedByUsers`)
      .then(res => {
        setProducts(res.data);
      })
      .catch(err => {
        console.log(err);
      });
  }, []);

  console.log(products)
  return (
    <div>
      {products.length === 0 ? (
        <div>No products found.</div>
      ) : (
        <MDBRow>
          {products.map((e, index) => {
            return (
              <MDBCol md="4" key={index}>
                <MDBCard  className="position-relative" >
                  <MDBCardImage
                    src={"https://localhost:44332/PicturesProduct/" + e.picture}
                    fluid
                    className="w-100"
                    alt={e.name}
                  />
   <MDBCardBody>
        <MDBRow className="d-flex justify-content-center align-items-center">
            <MDBCol>
                <MDBCardTitle className="text-center">{e.name}</MDBCardTitle>
                <div className="text-center">
                <a href={`/proizvod/${e.id}`}>
                    <MDBBtn outline color="primary" size="sm" className="mt-2">
                        OPŠIRNIJE
                    </MDBBtn>
                    </a>
                </div>
            </MDBCol>
        </MDBRow>
    </MDBCardBody>
                </MDBCard>
              </MDBCol>
            );
          })}
        </MDBRow>
      )}
   

    </div>
  );
} 
export default PreporuceniProizvodi;
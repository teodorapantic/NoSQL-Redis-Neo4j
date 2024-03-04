import React from "react";
import { useState, useEffect } from "react";

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
import { useParams } from 'react-router-dom';

function Kategorija() {
  const { IDCat } = useParams();
  const [products, setProducts] = useState([]);
 

  useEffect(() => {
    axios.put(`https://localhost:44332/GetAllProducts/${IDCat}`)
      .then(res => {
        setProducts(res.data);
      })
      .catch(err => {
        console.log(err);
      });
  }, [IDCat]);

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
                <MDBCard  className="position-relative mt-4" >
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
                        OPSIRNIJE
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


export default Kategorija;

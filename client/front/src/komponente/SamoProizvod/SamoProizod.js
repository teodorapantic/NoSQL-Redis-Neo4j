import React from "react";

import Narudzbina from "../Narudzbina/Narudzbina";
import Recenzija from "../Recenzija/Recenzija";
import Komentari from "../Komentari/Komentari";
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBIcon,
  MDBRipple,
  MDBBtn,
} from "mdb-react-ui-kit";
import { useState, useEffect } from "react";

import axios from "axios";
import { useParams } from 'react-router-dom';


function SamoProizvod() {

  const [product, setProduct] = useState();
  const [order, setOrder] = useState("");

  let { IdProduct } = useParams();

  const [loading, setLoading] = useState(true);

  const [following, setFollowing] = useState(localStorage.getItem(`following-${IdProduct}`) || false);
  const [user_info,setUserinfo]=useState(JSON.parse(localStorage.getItem('user-info')));

  const [showModal, setShowModal] = useState(false);
  const [showModal2, setShowModal2] = useState(false);

  const handleOpenModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };
  function ordershow() {
    setOrder(true);
  }
  function orderhide() {
    setOrder(false);
  }

  const handleOpenModal2 = () => {
    setShowModal2(true);
  };

  const handleCloseModal2= () => {
    setShowModal2(false);
  };

  let test = localStorage.getItem('user-info');
  let IDUser = null;
  if (test) {
    test = JSON.parse(test);
    IDUser = test.returnID;
  }

  const Follow = async () => {
    setFollowing(true);
    localStorage.setItem(`following-${IdProduct}`, true);
    try {
      const response = await axios.put(`https://localhost:44332/User/FollowProduct/${IDUser}/${IdProduct}`);
      console.log(response.data);
      alert(`Uspesno ste zapratili -${product.nameProduct}`);
    } catch (error) {
      console.error(error);
    }
  };

  const Unfollow = async () => {
    setFollowing(false);
    localStorage.removeItem(`following-${IdProduct}`);
    try {
      const response = await axios.delete(`https://localhost:44332/User/UnFollowProduct/${IDUser}/${IdProduct}`);
      console.log(response.data);
      alert("Uspeno ste odpratili proizovd");
    } catch (error) {
      console.error(error);
    }
  };


  useEffect(() => {
    axios.get(`https://localhost:44332/Product/GetMoreDetails/${IdProduct}`)
      .then(res => {
        console.log(res.data);
        setProduct(res.data);
        setLoading(false)
      })
      .catch(err => {
        console.log(err);
      });
  }, [IdProduct]);

  console.log(product)
  if (loading) return <p>Loading...</p>;

  return (

    <MDBContainer fluid>
      <MDBRow className="justify-content-center mb-0">
        <MDBCol md="12" xl="10">
          <MDBCard className="shadow-0 border rounded-3 mt-5 mb-3">
            <MDBCardBody>
              <MDBRow>
                <MDBCol md="12" lg="3" className="mb-4 mb-lg-0">
                  <MDBRipple
                    rippleColor="light"
                    rippleTag="div"
                    className="bg-image rounded hover-zoom hover-overlay"
                  >
                    <MDBCardImage
                      src={"https://localhost:44332/PicturesProduct/" + product.pictureProduct}
                      fluid
                      className="w-100"
                    />
                    <a href="#!">
                      <div
                        className="mask"
                        style={{ backgroundColor: "rgba(251, 251, 251, 0.15)" }}
                      ></div>
                    </a>
                  </MDBRipple>
                </MDBCol>
                <MDBCol md="6">
                  <h5>{product.nameProduct}</h5>
                  <div className="d-flex flex-row">

                  </div>
                  <div className="mt-1 mb-0 text-muted small">
                    <span>Proizvodjač :</span>
                    <span className="text-primary"> • </span>
                    <span>{product.manufacturer}</span>
                    <span className="text-primary">  </span>

                  </div>

                  <div className="mb-2 text-muted small">
                    <span>Reviews</span>
                    <span className="text-primary"> • </span>
                    <span>  {product.reviews}</span>
                    <span className="text-primary"> • </span>
                      <span style={{ color: product.rank >= 50 ? 'green' : 'inherit' }}>
                        {product.rank} %
                        {product.rank >= 50 && <span> !!! PREPORUČUJEMO PROIZVOD !!!</span>}
                      </span>
                  </div>



                    <div>
                    <MDBBtn color="primary" size="sm" onClick={handleOpenModal2}>Prikazi Recenzije</MDBBtn>
                    <Komentari show={showModal2} onHide={handleCloseModal2} nameProduct={product.nameProduct} idProduct={product.idProduct} />
                  </div>
                 

                  {user_info ? (<> <div>
                    <MDBBtn color="primary" size="sm" onClick={handleOpenModal}>Dodaj recenziju</MDBBtn>
                    <Recenzija show={showModal} onHide={handleCloseModal} nameProduct={product.nameProduct} idProduct={product.idProduct} />
                  </div></>):
                  (<></>)}
              
                  

                </MDBCol>


                <MDBCol md="6" lg="3" className="border-sm-start-none border-start">
                  <div>
                    {product.stored.map(market => (
                      <div key={market.id} className="d-flex flex-column align-items-start mb-1">
                        <div className="d-flex flex-row align-items-center">
                          <h4 className="mb-1 me-1">{market.market}-</h4>
                          <h4 className="mb-1 me-1 ml-auto">{market.price} din</h4>

                        </div>

                      </div>
                    ))}
                  </div>




                  {user_info ? (<>
                    <div className="d-flex flex-column mt-4">
                    <MDBBtn color="primary" size="sm" onClick={ordershow}> NARUCI</MDBBtn></div>
                  <Narudzbina show={order} onHide={orderhide} nameProduct={product.nameProduct} marketData={product.stored}></Narudzbina>
                  <div className="d-flex flex-column mt-4">
                    {following ? (
                      <MDBBtn color="danger" size="sm" onClick={Unfollow}>
                        Prestani da pratiš
                      </MDBBtn>
                    ) : (
                      <MDBBtn color="primary" size="sm" onClick={Follow}>
                        Zaprati Proizvod
                      </MDBBtn>
                    )}
                  </div>
                  
                  
                  
                  </>)
                  
                  :(<></>)}
                  
                  
                </MDBCol>

              </MDBRow>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>

      </MDBRow>


    </MDBContainer>
  )
}



export default SamoProizvod;

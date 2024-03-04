import React from 'react';
import { Modal, Form, FormControl, Button, InputGroup, DropdownButton, Dropdown } from 'react-bootstrap';
import { MDBBtn } from 'mdb-react-ui-kit';
import { MDBIcon } from 'mdb-react-ui-kit';
import { useState, useEffect } from "react";
import axios from 'axios';




const Recenzija = (props) => {
    const [text, setText] = useState('');
   
    const [idProduct, setIdProduct] = useState('');
    const [recommend, setRecommend] = useState(null);
    const [errorMessage, setErrorMessage] = useState('');
    let test = localStorage.getItem('user-info');
    let IDUser = null;
    let username = null;
    if (test) {
        test = JSON.parse(test);
        IDUser = test.returnID;
        username = test.userName;
    }
    const addComment = async() => {
        if (!text) {
            setErrorMessage('Molim Vas unesite komentar.');
            return;
          }
      
          if (recommend === null) {
            setErrorMessage('Da li vam se ovaj proizvod dopao?Kliknite like.');
            return;
          }
          setIdProduct(props.idProduct);

        try {
            const response = await axios.post(`https://localhost:5001/Review/ReviewPoduct/${text}/${username}/${idProduct}/${recommend}`);


            if (response.status !== 200) {
                console.error(`API odgovor nije uspeo: ${response.status}`);
                return;
            }

            const data = response.data;


            
            alert("Dodali ste recenziju");
            console.log(response);
        } catch (error) {
            console.error(error);
        }
    }

      


    const handleLike = () => {
        setRecommend(true);
    };
    
    const handleDislike = () => {
        setRecommend(false);
    };


    return (
        <Modal {...props} size="lg" centered>
            <Modal.Header closeButton>
                <Modal.Title><strong>{props.nameProduct} *RECENZIJE*</strong></Modal.Title>
            </Modal.Header>
            <Modal.Body>
            {errorMessage && <div style={{ color: 'red' }}>{errorMessage}</div>}
                <Form>
                    <Form.Group>
                        <Form.Label><strong>komentar:</strong></Form.Label>
                        <Form.Control as="textarea" style={{ height: '100%' }} placeholder="Unesite neku recenziju"
                            onChange={e => setText(e.target.value)} />

                        <Form.Group className="d-flex">
                        <Button color="primary" onClick={handleDislike}>
    <MDBIcon fas icon="thumbs-down" />
</Button>
                            <Button color="primary" onClick={handleLike}>
    <MDBIcon fas icon="thumbs-up" />
</Button>
                        </Form.Group>
                    </Form.Group>
                    <Form.Group>
                        <Button color="primary" onClick={addComment}>Dodaj komentar</Button>
                    </Form.Group>
                </Form>
            </Modal.Body>
        </Modal>
    )
};

export default Recenzija;

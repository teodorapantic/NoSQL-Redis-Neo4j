import React from 'react';
import './Komentari.css';
import { Modal, Form, FormControl, Button, InputGroup, DropdownButton, Dropdown } from 'react-bootstrap';
import { MDBBtn } from 'mdb-react-ui-kit';
import { MDBIcon } from 'mdb-react-ui-kit';
import { useState, useEffect } from "react";
import axios from 'axios';
import { useParams } from 'react-router-dom';

const Komentari = (props) => {
    const [username, setUsername] = useState('');
    const [text, setText] = useState('');
    const [comment, setComment] = useState();
    const [loading, setLoading] = useState(true);
    let { IdProduct } = useParams();


    useEffect(() => {
        axios.get(`https://localhost:44332/Review/GetReview/${IdProduct}`)
            .then(res => {
                console.log(res.data);
                setComment(res.data);
                setLoading(false)
            })
            .catch(err => {
                console.log(err);
            });
    }, [IdProduct]);

    if (loading) return <p>Loading...</p>;


    return (
        <Modal {...props} size="lg" centered>
            <Modal.Header closeButton>
                <Modal.Title><strong>{props.nameProduct} *KOMENTARI*</strong></Modal.Title>
            </Modal.Header>
            <Modal.Body>
  {comment.map(({ username, text, recommend }) => (
    <p>
      <strong>{username} :: </strong> 
      <span className={`recommend-${recommend}`}>{text}</span>
    </p>
  ))}
</Modal.Body>
        </Modal>
    )
};

export default Komentari;


import React from 'react';
import './Narudzbina.css';
import { Modal, Form, FormControl, Button, InputGroup, DropdownButton, Dropdown } from 'react-bootstrap';
import axios from 'axios';
import { useState, useEffect } from "react";


const Narudzbina = (props) => {
    const { show, onHide, nameProduct, price, marketData } = props;
    const [selectedMarket, setSelectedMarket] = useState(null);
    const [kolicina, setKolicina] = useState(0);
    const [dostavljac, setDostavljac] = useState('');
    const [market, setMarket] = useState('');
    const [cena, setCena] = useState(0);
    const [lokacija, setLokacija] = useState('');
    const [brtel, setBrTel] = useState('');

    const [deliveries, setDeliveries] = useState('');
    


    let test = localStorage.getItem('user-info');
    let IDUser = null;
    if (test) {
        test = JSON.parse(test);
        IDUser = test.returnID;
    }

    useEffect(() => {

        axios.get("https://localhost:44332/Delivery/GetAll")
            .then(res => {
                console.log(res)
                setDeliveries(res.data)
            })

            .catch(err => {
                console.log(err)
            })
    }, [])

    console.log(deliveries);


    async function order(selectedMarket, nameProduct, cena, kolicina, lokacija, brtel, dostavljac, IDUser) {
        console.log(selectedMarket, nameProduct, cena, kolicina, lokacija, brtel, dostavljac, IDUser);


        if (!selectedMarket || !nameProduct || !cena || !kolicina || !lokacija || !brtel || !dostavljac || !IDUser) {
            console.error("Nedostaju neke od obaveznih varijabli");
            return;
        }

        try {
            const response = await axios.post(`https://localhost:44332/Order/MakeOrder/${selectedMarket}/${nameProduct}/${cena}/${kolicina}/${lokacija}/${brtel}/${dostavljac}/${IDUser}`);


            if (response.status !== 200) {
                console.error(`API odgovor nije uspeo: ${response.status}`);
                return;
            }

            const data = response.data;


            
            alert("Narudzbina je poslata dostavljacu");
            console.log(response);
        } catch (error) {
            console.error(error);
        }
    }



    const handlePriceCalculation = () => {
        return  kolicina * (selectedMarket ? selectedMarket.price : 0);
    }


    const handleSelectChange = (event) => {
        setSelectedMarket(marketData.find(market => market.market === event.target.value));
    }


    return (
        <Modal {...props} size="lg" centered>
            <Modal.Header closeButton>
                <Modal.Title><strong>{props.nameProduct}</strong></Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form.Group>
                    <Form.Label><strong> Narucujete iz: </strong></Form.Label>
                    <Form.Control as="select" value={selectedMarket ? selectedMarket.market : ""} onChange={handleSelectChange}>
                        <option value="">Izaberite market</option>
                        {marketData.map((market, index) => (
                           <option key={index} value={market.market} disabled={!market.available}  className={market.sale ? 'sale' : ''}>
                           {market.market} - cena : {market.price} din  {market.sale ? 'AKCIJSKA CENA' : ''} {!market.available ? "(Trenutno nema na stanju u ovom marketu)" : ""}
                       </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form>

                    <Form.Group>
                        <Form.Label><strong>Unesite Vase informacije</strong></Form.Label>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label><strong>Broj telefona</strong></Form.Label>
                        <Form.Control value={brtel} onChange={e => setBrTel(e.target.value)} type="text" placeholder="Broj telefona" />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label><strong>Lokacija</strong></Form.Label>
                        <Form.Control value={lokacija} onChange={e => setLokacija(e.target.value)} type="text" placeholder="Lokacija" />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label><strong>Dostavljač</strong></Form.Label>
                        <Form.Control as="select" value={dostavljac} onChange={e => setDostavljac(e.target.value)}>
                            <option value="">Izaberite dostavljača</option>
                            {deliveries && deliveries.length > 0 && deliveries.map(delivery => (
                                <option key={delivery.id} value={delivery.name}>{delivery.name}</option>
                            ))}
                        </Form.Control>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label><strong>Kolicina</strong></Form.Label>
                        <Form.Control value={kolicina} onChange={e => setKolicina(e.target.value)} type="number" placeholder="Unesite kolicinu" />
                    </Form.Group>

                    <p><strong> Ukupna cena  ove porudzbine: {handlePriceCalculation(selectedMarket)}</strong></p>
                    <Button onClick={() => order(selectedMarket.market, nameProduct, handlePriceCalculation(selectedMarket), kolicina, lokacija, brtel, dostavljac, IDUser)}>Narucite</Button>

                </Form>
            </Modal.Body>
        </Modal>
    )
}
export default Narudzbina;
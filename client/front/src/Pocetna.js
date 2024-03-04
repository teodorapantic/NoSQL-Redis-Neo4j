import React, { useEffect, useState } from 'react';
import PreporuceniProizvodi from './komponente/PreporuceniProizvodi/PreporuceniProizvodi';




function Pocetna() {
    return (
        <>
      <div style={{ textAlign: 'center', fontWeight: 'bold', fontSize: 'large' }}>
       Pogledajte proizvode preporucene od strane drugih korisnika
      </div>
      <div>
      <PreporuceniProizvodi />
      </div>
      </>
    );
  }
  
  export default Pocetna;
  
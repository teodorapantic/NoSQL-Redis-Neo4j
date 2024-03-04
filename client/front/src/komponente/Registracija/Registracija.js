import React, {useState} from 'react';

import {
  MDBBtn,
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBInput,
  MDBCheckbox,
  MDBValidation,
  MDBValidationItem,
  MDBSwitch
 
}
from 'mdb-react-ui-kit';


function Registracija() {

  const [formValue, setFormValue] = useState({
    fname: '',
    lname: '',
    username: '',
    password: '',
    number:'',
    location:''
    
  });

  const onChange = (e) => {
    setFormValue({ ...formValue, [e.target.name]: e.target.value });
    
    
  };

  

  const handleSubmit = (e) => {
    if (formValue.fname && formValue.lname && formValue.username && formValue.password) {
      
      const checkbox = document.getElementById('invalidCheck');
      if (checkbox.checked) {
        
        register();

      } else {
        const checkboxValidationItem = document.getElementById('invalidCheck');
        checkboxValidationItem.setAttribute('valid', true);
      }
    } else {
      const fnameValidationItem = document.getElementById('validationCustom01');
      const lnameValidationItem = document.getElementById('validationCustom02');
      const usernameValidationItem = document.getElementById('validationCustom03');
      const passwordValidationItem = document.getElementById('validationCustom04');
      const numberValidationItem = document.getElementById('validationCustom05');
      const locationValidationItem = document.getElementById('validationCustom06');
  
      if (!formValue.fname) {
        fnameValidationItem.setAttribute('invalid', true);
      }
      if (!formValue.lname) {
        lnameValidationItem.setAttribute('invalid', true);   
      }
      if (!formValue.usernameValidationItem) {
        usernameValidationItem.setAttribute('invalid', true);   
    }
    if (!formValue.passwordValidationItem) {
      passwordValidationItem.setAttribute('invalid', true);   
}


}};

async function register()
{
          console.log(formValue);    

            let username=formValue.username;
            let password=formValue.password;
            let Name = formValue.fname;
            let SurName=formValue.lname;
            let PhoneNumber=formValue.number;
            let Location=formValue.location;

            let result = await fetch('https://localhost:44332/User/CreateUser/' + username + '/' + password + '/' + Name + '/' + SurName+ '/' + PhoneNumber + '/' + Location, {
              method: 'POST'
            })
            .then(response => {
              console.log(response);
              if (response.ok) {
                alert("Uspeno ste se registrovali, sada mozete da se ulogujete i koristite aplikaciju!");
              } else {
                throw new Error('Something went wrong');
              }
            })
            .catch(error => {
              console.log(error);
              console.log("GRESKAAAA");
              alert("Došlo je do greške prilikom registracije. Molimo pokušajte ponovo.");
            });
            
}

  return (
    <MDBContainer fluid className='bg-secondary p-2 text-dark bg-opacity-10 d-flex justify-content-center' >

      <MDBCard className='text-black m-5 align-items-center  '  style={{borderRadius: '12px', maxWidth: '900px' }}>
        <MDBCardBody size='lg' >
          <MDBRow>
          <MDBValidation className='row g-3'>
            <MDBCol  className='order-2 order-lg-1 d-flex flex-column align-items-center mx-4 '>
            <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-2">Registruj se</p>
            <MDBRow className='pl-5'>

            <MDBCol className='ml-5'>

            <div className="d-flex flex-row align-items-center mb-4 ml-4 ">

              <MDBValidationItem feedback='Unesite ime' invalid>
              <MDBInput
                value={formValue.fname}
                name='fname'
                onChange={onChange}
                id='validationCustom01'
                required
                label='Ime'
              />
              </MDBValidationItem>
              </div>



              <div className="d-flex flex-row align-items-center mb-4">
              <MDBValidationItem feedback="Unesite korisničko ime" invalid >
              <MDBInput
                  value={formValue.username}
                  name='username'
                  onChange={onChange}
                  className='form-control'
                  id='validationCustom03'
                  label='Korisničko ime'
                  required
                  
                />
              
              </MDBValidationItem>
              </div>

              <div className="d-flex flex-row align-items-center mb-4">
              <MDBValidationItem feedback="Unesite broj telefona" invalid >
              <MDBInput
                  value={formValue.number}
                  name='number'
                  onChange={onChange}
                  className='form-control'
                  id='validationCustom05'
                  label='Broj telefona'
                  required
                  
                />
              
              </MDBValidationItem>
              </div>
          </MDBCol>
          <MDBCol>



          <div className="d-flex flex-row align-items-center mb-4">
            <MDBValidationItem feedback='Unesite prezime' invalid>
            <MDBInput
              value={formValue.lname}
              name='lname'
              onChange={onChange}
              id='validationCustom02'
              required
              label='Prezime'
              
            />
            </MDBValidationItem>
            </div>


            <div className="d-flex flex-row align-items-center mb-4">
            <MDBValidationItem feedback='Unesite lozinku' invalid>
            <MDBInput
              value={formValue.password}
              type='password'
              onChange={onChange}
              id='validationCustom04'
              required
              label='Lozinka'
              name='password'
            />
            </MDBValidationItem>
            </div>

            <div className="d-flex flex-row align-items-center mb-4">
            <MDBValidationItem feedback="Unesite adresu" invalid >
            <MDBInput
                value={formValue.location}
                name='location'
                onChange={onChange}
                className='form-control'
                id='validationCustom06'
                label='Adresa'
                required
                
              />
            
            </MDBValidationItem>
            </div>

          </MDBCol>



      </MDBRow>
                 
      <div className='mb-2'>
        <MDBValidationItem className='col-12 mb-3' feedback='Morate da prihvatite pre registracije.' invalid>
           <MDBCheckbox label='Slažem se sa uslovima korišćenja'   id='invalidCheck' required />
        </MDBValidationItem>
      </div>



      <MDBBtn type='submit' size='lg'  className='mb-4' onClick={handleSubmit} >REGISTRACIJA</MDBBtn>
            
              
    </MDBCol>
            
    <MDBCol  className='order-1 order-lg-2 d-flex align-items-center'>
      <MDBCardImage src='https://img.freepik.com/free-vector/seasonal-sale-discounts-presents-purchase-visiting-boutiques-luxury-shopping-price-reduction-promotional-coupons-special-holiday-offers-vector-isolated-concept-metaphor-illustration_335657-2766.jpg' fluid/>
    </MDBCol>

</MDBValidation>
          </MDBRow>
        </MDBCardBody>
      </MDBCard>

    </MDBContainer>
  );
}

export default Registracija;
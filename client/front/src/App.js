import logo from './logo.svg';
import './App.css';
import {Route} from 'react-router-dom';
import Navigacija from './komponente/Navigacija/Navigacija';
import Logovanje from './komponente/Logovanje/Logovanje';
import Rute from './Routes';

function App() {
  return (
    /*<div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>*/
    <div  className='bg-secondary bg-opacity-10' style={{width: '100%', height: '100%'}}>
    
      
        <Rute />
 
      
    </div>
  );
}

export default App;

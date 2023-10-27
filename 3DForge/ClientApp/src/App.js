import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import AuthorizationPage from './pages/AuthorizationPage/AuthorizationPage';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/auth' element={<AuthorizationPage />} />
        <Route path='/register' element={<RegistrationPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
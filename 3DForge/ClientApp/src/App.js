import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import AuthorizationPage from './pages/AuthorizationPage/AuthorizationPage';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';
import NavigationHeader from './components/NavigationHeader/NavigationHeader';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<NavigationHeader />}>
          <Route index element={<HomePage />} />
          <Route path='user/info' element={<></>} />
          <Route path='user/edit' element={<></>} />
        </Route>
        <Route path='auth' element={<AuthorizationPage />} />
        <Route path='register' element={<RegistrationPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
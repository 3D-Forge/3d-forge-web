import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import AuthorizationPage from './pages/AuthorizationPage/AuthorizationPage';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';
import AccountPageLayout from './components/AccountPageLayout/AccountPageLayout';
import UserInfoPage from './pages/UserInfoPage/UserInfoPage';
import { UserAPI } from './services/api/UserAPI';
import UserEditPage from './pages/UserEditPage/UserEditPage';
import ResetPasswordPage from './pages/ResetPasswordPage/ResetPasswordPage';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<AccountPageLayout />}>
          <Route index element={<HomePage />} />
          <Route path='user/info' element={<UserInfoPage />} />
          <Route path='user/edit' element={<UserEditPage />} />
        </Route>
        <Route path='auth' element={<AuthorizationPage />} />
        <Route path='register' element={<RegistrationPage />} />
        <Route path='reset-password' element={<ResetPasswordPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
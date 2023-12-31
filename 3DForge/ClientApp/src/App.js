import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import AuthorizationPage from './pages/AuthorizationPage/AuthorizationPage';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';
import AccountPageLayout from './components/AccountPageLayout/AccountPageLayout';
import UserInfoPage from './pages/UserInfoPage/UserInfoPage';
import UserEditPage from './pages/UserEditPage/UserEditPage';
import ResetPasswordPage from './pages/ResetPasswordPage/ResetPasswordPage';
import ModelPage from './pages/ModelPage/ModelPage';
import CatalogPage from './pages/CatalogPage/CatalogPage';
import AdminPage from './pages/AdminPage/AdminPage';
import CartPage from './pages/CartPage/CartPage';
import CatalogModerationPage from './pages/CatalogModerationPage/CatalogModerationPage';
import MyPublicationsPage from './pages/MyPublicationsPage/MyPublicationsPage';
import { ContextProvider } from './ContextProvider';

const App = () => {
    return (
        <ContextProvider>
            <BrowserRouter>
                <Routes>
                    <Route path='' element={<AccountPageLayout />}>
                        <Route index element={<HomePage />} />
                        <Route path='user/info' element={<UserInfoPage />} />
                        <Route path='user/publications' element={<MyPublicationsPage />} />
                        <Route path='user/edit' element={<UserEditPage />} />
                        <Route path='catalog' element={<CatalogPage />} />
                        <Route path='catalog/:id' element={<ModelPage />} />
                        <Route path='cart' element={<CartPage />} />
                        <Route path='admin' element={<AdminPage />}>
                            <Route index element={<></>} />
                            <Route path='catalog' element={<CatalogModerationPage />} />
                        </Route>
                    </Route>
                    <Route path='auth' element={<AuthorizationPage />} />
                    <Route path='register' element={<RegistrationPage />} />
                    <Route path='reset-password' element={<ResetPasswordPage />} />
                </Routes>
            </BrowserRouter>
        </ContextProvider>
    );
}

export default App;
import React, { Suspense } from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './resources/index.css';
import './resources/logo.ico';

ReactDOM.createRoot(document.getElementById('root')).render(
    <Suspense fallback={<div>Loading...</div>}>
        <App />
    </Suspense>
);

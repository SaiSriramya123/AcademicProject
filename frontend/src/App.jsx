import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext'; 
import { Home } from './pages/Home';
import { RoleSelection } from './pages/RoleSelection';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="app-container">
          {/* The Routes component looks at the URL. 
            If the path is "/", it renders the Home (Landing Page).
          */}
          <Routes>
            {/* Landing Page */}
            <Route path="/" element={<Home />} />

            {/* Registration & Role Selection */}
            <Route path="/register" element={<RoleSelection />} />

            {/* The Navbar inside <Home /> now has access to AuthProvider 
               because it sits inside this tree.
            */}
            
            <Route path="/login" element={<div>Login Page</div>} />
            <Route path="*" element={<div className="p-20 text-center">404 - Not Found</div>} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
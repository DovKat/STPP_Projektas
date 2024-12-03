import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ForumPage from './pages/ForumPage';
import Header from './components/Header';
import Footer from './components/Footer';

function App() {
  return (
    <AuthProvider>
      <Router>
      <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-grow p-4">
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route 
            path="/forums" 
            element={
              <ProtectedRoute>
                <ForumPage />
              </ProtectedRoute>
            } 
          />
        </Routes>
        </main>
      <Footer />
      </div>
      </Router>
    </AuthProvider>
  );
}

export default App;

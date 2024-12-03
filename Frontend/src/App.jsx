import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ForumPage from './pages/ForumPage';
import Header from './components/Header';
import Footer from './components/Footer';
import PostListPage from './pages/PostListPage';
import PostDetailsPage from './pages/PostDetailsPage';
import ContactPage from './pages/ContactPage'; 
import HomePage from './pages/HomePage';
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
          <Route 
            path="/forums/:forumId" 
            element={
            <ProtectedRoute>
                <PostListPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/forums/:forumId/posts/:postId" 
            element={
              <ProtectedRoute>
                <PostDetailsPage /> 
              </ProtectedRoute>
            }
          />
          <Route 
            path="/home"
            element={
              <ProtectedRoute>
                <HomePage /> 
              </ProtectedRoute>
            }
          />
          <Route 
            path="/contact"
            element={
              <ProtectedRoute>
                <ContactPage /> 
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

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ForumPage from './pages/ForumPage';
import Layout from './components/layout';
import PostListPage from './pages/PostListPage';
import PostDetailsPage from './pages/PostDetailsPage';
import ContactPage from './pages/ContactPage'; 
import HomePage from './pages/HomePage';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* Unauthenticated Routes */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          
          {/* Routes with Layout */}
          <Route 
            path="/" 
            element={
              <ProtectedRoute>
              <Layout>
                <HomePage />
              </Layout>
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/forums" 
            element={
              <ProtectedRoute>
                <Layout>
                  <ForumPage />
                </Layout>
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/forums/:forumId" 
            element={
              <ProtectedRoute>
                <Layout>
                  <PostListPage />
                </Layout>
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/forums/:forumId/posts/:postId" 
            element={
              <ProtectedRoute>
                <Layout>
                  <PostDetailsPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          <Route 
            path="/contact"
            element={
              <ProtectedRoute>
                <Layout>
                  <ContactPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          {/* Fallback 404 */}
          <Route path="*" element={<Layout><div>404 Page Not Found</div></Layout>} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}


export default App;

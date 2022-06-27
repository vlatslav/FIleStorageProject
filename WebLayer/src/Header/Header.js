import React from "react";
import {BrowserRouter as Router, Link, Route, Routes} from "react-router-dom";
import File from "../FileComponent/File";
import AdminPage from "../AdminComponent/Admin";
import Registration from "../Registration/Registration";
import Authorization from "../Authorization/Authorization";
import 'bootstrap/dist/css/bootstrap.css';

function Header() {
    const signOut = () => {
        localStorage.clear()
        window.location.href = "http://localhost:3000/signin";
    }
    return (
        <>
            <Router>
            <nav className="navbar navbar-expand-sm bg-light navbar-dark">
                <ul className="navbar-nav">
                    {localStorage.hasOwnProperty('UserId') ? (
                        <li className="nav-item- m-1">
                            <Link style={{ textDecoration: 'none' }} className="btn btn-light btn-outline-primary" to="/files">
                                Files
                            </Link>
                        </li>
                    ) : (
                        <li className="nav-item- m-1">
                            <Link style={{ textDecoration: 'none' }} className="btn btn-light btn-outline-primary" to="/signin">
                                Sign in
                            </Link>
                        </li>
                    )}
                    {localStorage.getItem('roles')?.includes("Administrator") ? (

                        <li className="nav-item- m-1">
                            <Link style={{ textDecoration: 'none' }}  className="btn btn-light btn-outline-primary" to="/admin">
                                Admin Menu
                            </Link>
                        </li>

                    ) : (
                        console.log('user')
                    )}
                    {!localStorage.hasOwnProperty('UserId') ?
                        <li className="nav-item- m-1">
                            <Link style={{ textDecoration: 'none' }} className="btn btn-light btn-outline-primary" to="/signup">
                                Sign up
                            </Link>
                        </li> :
                        console.log('user')}

                    {localStorage.hasOwnProperty('UserId') ?
                        <li className="nav-item- m-1">
                            <Link style={{ textDecoration: 'none' }} className="btn btn-light btn-outline-primary" to="/signin" onClick={signOut}>
                                Sign out
                            </Link>
                        </li> :
                        console.log('')}
                    </ul>
            </nav>
                <Routes>
                    <Route path="/files" element={<File/>}/>
                    <Route path="/admin" element={<AdminPage/>}/>
                    <Route path="/signup" element={<Registration/>} />
                    <Route path="/signin" element={<Authorization/>} />
                </Routes>
            </Router>
        </>
    );
}
export default Header;
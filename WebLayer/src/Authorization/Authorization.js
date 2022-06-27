import React, {useEffect, useState} from "react";
import {Button, Form} from "react-bootstrap";
import {variables} from "../Variables/Variables";

function Authorization() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [authResponse,setAuthResponse] = useState([]);
    const [users,setUsers] = useState([]);
    const checkPassword = (nickname,password) =>
    {
        signInUser(nickname,password);
        const user = users?.find(x => x.userName === nickname);
        if(user !== null){
            localStorage.setItem('UserId', user.UserId);
            localStorage.setItem('roles', user.roles);
        }
        console.log("Was authorized");
        window.location.href = "http://localhost:3000/files";
    }
    useEffect(() => {
        fetch(variables.API_URL + 'Authentication')
            .then(response => response.json())
            .then(data => {
                setUsers(data);
            })
    }, [])

    const signInUser = (userName,password) => {
        fetch(variables.API_URL + 'Authentication/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userName:userName,
                Password:password
            })

        }).then(response => response.json())
            .then((result) => {
                if(result.token){
                    console.log(result.token);
                    setAuthResponse(result.token);
                    localStorage.setItem("user", JSON.stringify(result.token));
                }
            }).catch((error) => {
            console.log(error.response)
        })
    }
    const authorizeUserClicked = () => {

        if (userName !== '' && password !== '') {
            checkPassword(userName, password)
        } else {
            alert("You forgot to fill some fields.")
        }
    }
        return (
            <>
                <div style={{display: 'flex', justifyContent: 'center'}}>
                    <Form>
                        <Form.Group className="mb-3" controlId="formBasicName">
                            <Form.Label style={{paddingRight: '3%'}} >User name</Form.Label>
                            <Form.Control type="name"
                                          placeholder="Enter user name"
                                          value={userName}
                                          onChange={(e) =>
                                              setUserName(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                            </Form.Text>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formBasicPassword">
                            <Form.Label style={{paddingRight: '5.2%'}} >Password</Form.Label>
                            <Form.Control type="password"
                                          placeholder="Password"
                                          value={password}
                                          onChange={(e) =>
                                              setPassword(e.target.value)}
                            />
                        </Form.Group>
                        <Button variant="primary"
                                onClick={authorizeUserClicked}>
                            Submit
                        </Button>
                        <Button style={{paddingLeft: '3%'}} variant="link" href='/signup'>I don't have an account  </Button>
                        <Button style={{paddingLeft: '3%'}} variant="link" href='/files'>   Continue as anon account</Button>
                    </Form>
                </div>

            </>
        )
}

export default Authorization;
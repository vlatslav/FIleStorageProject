import React, { useState} from "react";
import {Button, Form} from "react-bootstrap";
import {variables} from "../Variables/Variables";

function Registration() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [nickName, setNickName] = useState('')
    const [confirmPass, setConfirmPass] = useState('')




    const confirmPassword = () => {
        if (validatePassword()) {
            if (confirmPass === password) {
                return true;
            } else {
                alert("Password didnt match")
            }
        } else {
            alert("Password must contain at least:\n 8 characters,\n 1 capital letter,\n 1 lower letter,\n 1 special symbol ")
        }
    }

    const validatePassword = () => {
        const lowerCaseLetters = /[a-z]/g;
        const upperCaseLetters = /[A-Z]/g;
        const numbers = /[0-9]/g;
        if (password.length >= 8 &&
            password.match(lowerCaseLetters) &&
            password.match(upperCaseLetters) &&
            password.match(numbers))
            return true;
        return false;
    }


    const addUserClicked = () => {

        if (email !== ''
            && firstName !== ''
            && lastName !== ''
            && password !== ''
            && nickName !== ''
            && confirmPassword !== '') {

            if (confirmPassword()) {
                createAndSendToDbUser(email, firstName, lastName, nickName, password);
                window.location.href = "http://localhost:3000/signin";

            }

        } else {
            alert("You forgot to fill some fields.")
        }


    }
    const createAndSendToDbUser = (email, firstname,lastname,nickName,password) => {
        fetch(variables.API_URL + 'Authentication/registration', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userName: nickName,
                email:email,
                firstName:firstname,
                lastName:lastname,
                roles: ["User"],
                password:password
            })

        })
            .then(res => res.json())
            .then((result) => {
            }, (error) => {
            })
    }


        return (
            <>
                <div style={{display: 'flex', justifyContent: 'center'}}>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Email address</Form.Label>
                            <Form.Control type="email"
                                          placeholder="Enter email"
                                          value={email}
                                          onChange={(e) =>
                                              setEmail(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Nick Name</Form.Label>
                            <Form.Control type="text"
                                          placeholder="Enter Nick Name"
                                          value={nickName}
                                          onChange={(e) =>
                                              setNickName(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>First Name</Form.Label>
                            <Form.Control type="text"
                                          placeholder="Enter First Name"
                                          value={firstName}
                                          onChange={(e) =>
                                              setFirstName(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Last Name </Form.Label>
                            <Form.Control type="text"
                                          placeholder="Enter Last Name"
                                          value={lastName}
                                          onChange={(e) =>
                                              setLastName(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Password</Form.Label>
                            <Form.Control type="password"
                                          placeholder="Password"
                                          value={password}
                                          onChange={(e) =>
                                              setPassword(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Confirm Password</Form.Label>
                            <Form.Control type="password"
                                          placeholder="Password"
                                          value={confirmPass}
                                          onChange={(e) =>
                                              setConfirmPass(e.target.value)}
                            />
                        </Form.Group>

                        <Button variant="primary"
                                onClick={addUserClicked}>
                            Submit
                        </Button>
                        <Button variant="link" href='/signin'>I have an account</Button>
                    </Form>
                </div>

            </>
        )
    }




export default Registration;


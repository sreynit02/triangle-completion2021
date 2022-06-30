using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;

public class TrialScript : MonoBehaviour {

    // Participant info
    public int pNum;
    public bool bothPresentCondition;

    // Trials and trial types
    public int nBlocks = 11;
    public int trialCount;
    public int landOnly = 1;
    public int selfOnly = 2;
    public int combCue = 3;
    public int confCue = 4;
    public int trialType;
    public int rightNorth = 1;
    public int rightSouth = 2;
    public int leftNorth = 3;
    public int leftSouth = 4;
    public int homeCode;
    public int outCode;

    public int[] trialArray;
    public int[] homeArray;
    public int[] outBoundArray;

    // Home and outbound post locations
    public Vector3 homeRight;
    public Vector3 homeLeft;
    public Vector3 outBoundRight;
    public Vector3 outBoundLeft;

    // Time tracking
    public float startTime;
    public float responseTime;

    // Object Variables
    public GameObject outBoundPost;
    public GameObject resetPost;
    public GameObject resetPostWhite;
    public GameObject homePost;
    public GameObject rock;
    public GameObject tower;
    public GameObject tree;
    public GameObject player;
    public GameObject canvasBlack;
    public GameObject canvasVideo;
    public GameObject colliderSphere;
    public GameObject homePostCollider;
    public GameObject outPostCollider;
    public GameObject headingTracker;
    public GameObject groundPlane;

    // Position (and heading) vectors/angles
    public Vector3 playerPos;
    public Vector3 homePostLoc;
    public Vector3 outBoundPostLoc;
    public Vector3 outBoundPostFINLoc;

    // Video variables
    public RawImage rawImage;
    public VideoPlayer videoPlayerOne;
    public VideoPlayer videoPlayerTwo;
    public VideoPlayer videoPlayerThree;
    public VideoPlayer videoPlayerFour;
    public VideoPlayer videoPlayerFive;
    public VideoPlayer videoPlayerSix;
    public VideoPlayer videoPlayerSeven;
    public VideoPlayer videoPlayerEight;
    public VideoPlayer videoPlayerNine;
    public VideoPlayer videoPlayerTen;
    public VideoPlayer videoPlayerEleven;
    public VideoPlayer videoPlayerTwelve;
    public VideoPlayer videoPlayerThirteen;
    public VideoPlayer videoPlayerFourteen;
    public VideoPlayer videoPlayerFifteen;
    public VideoPlayer videoPlayerSixteen;

    // Counting tasks
    public GameObject FifteenStart;
    public GameObject ThirtyStart;
    public GameObject FortyFiveStart;
    public GameObject SixtyStart;
    public GameObject SeventyFiveStart;
    public GameObject NinetyStart;
    public int fifteenStart = 1;
    public int thirtyStart = 2;
    public int fortyFiveStart = 3;
    public int sixtyStart = 4;
    public int seventyFiveStart = 5;
    public int ninetyStart = 6;
    public int[] countStartArr;
         
    // Rotate landmarks in conflict condition
    public void rotateLandmarks()
    {
        rock.transform.RotateAround(outBoundPostFINLoc, Vector3.up, 20f);
        tree.transform.RotateAround(outBoundPostFINLoc, Vector3.up, 20f);
        tower.transform.RotateAround(outBoundPostFINLoc, Vector3.up, 20f);
    }

    // Undo the rotation from conflict condition
    public void undoRotate()
    {
        rock.transform.RotateAround(outBoundPostFINLoc, Vector3.down, 20f);
        tower.transform.RotateAround(outBoundPostFINLoc, Vector3.down, 20f);
        tree.transform.RotateAround(outBoundPostFINLoc, Vector3.down, 20f);
    }

    // Functions to rotate post locations
    public void rotateHomeNorthR()
    {
        homePost.transform.RotateAround(outBoundPostFINLoc, Vector3.up, -30f);
    }
    public void rotateHomeSouthR()
    {
        homePost.transform.RotateAround(outBoundPostFINLoc, Vector3.up, -15f);
    }
    public void rotateOutNorthR()
    {
        outBoundPost.transform.RotateAround(outBoundPostFINLoc, Vector3.up, -45f);
    }
    public void rotateOutSouthR()
    {
        outBoundPost.transform.RotateAround(outBoundPostFINLoc, Vector3.up, 0f);
    }
    public void rotateHomeNorthL()
    {
        homePost.transform.RotateAround(outBoundPostFINLoc, Vector3.down, -30f);
    }
    public void rotateHomeSouthL()
    {
        homePost.transform.RotateAround(outBoundPostFINLoc, Vector3.down, -15f);
    }
    public void rotateOutNorthL()
    {
        outBoundPost.transform.RotateAround(outBoundPostFINLoc, Vector3.down, -45f);
    }
    public void rotateOutSouthL()
    {
        outBoundPost.transform.RotateAround(outBoundPostFINLoc, Vector3.down, -0f);
    }

    // Write to data (text) file
    public void CreateText()
    {
        // Path
        string path = Application.dataPath + "/Data " + pNum + " .txt";
        // Create if not already existing
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Participant " + pNum + " Data\n\n");
        }
        // Content
        string dateAndTime = "Date & Time: " + System.DateTime.Now + "\n\n";
        string condition = "Condition: " + bothPresentCondition + "\n\n";
        if (trialCount == 0)
        {
            File.AppendAllText(path, dateAndTime);
            File.AppendAllText(path, condition);
        }
		if (trialCount != 0)
        {
            File.AppendAllText(path, "Trial " + (trialCount - 4) + ", Trial Type: " + trialType + "\n");
			File.AppendAllText(path, "Second Post Location" + outBoundPostLoc + "\n");
            File.AppendAllText(path, "Home Location: " + homePostLoc + "\n");
            File.AppendAllText(path, "Response Location: " + playerPos + "\n");
			File.AppendAllText(path, "Heading Angle: " + GameObject.Find("Heading Tracker").GetComponent<HeadingAngleTracker>().headingAngleError + "\n");
            File.AppendAllText(path, "Response Time: " + responseTime + "\n\n");
        }
    }

    // Trigger checks
    public IEnumerator axisChecker()
    {
        while(Input.GetAxis("Trigger") == 0 || Input.GetMouseButtonDown(1) ) // Haley add 
        {
            yield return null;
        }
    }

    public IEnumerator waitForTrigger()
    {
        while(Input.GetAxis("Trigger") != 0)  
        {
            yield return null;
        }
    }

    // Post checks
    public IEnumerator arriveAtHomePost()
    {
        while(homePost.activeInHierarchy == true)
        {
            yield return null;
        }
    }

    public IEnumerator arriveAtOutPost()
    {
        while(outBoundPost.activeInHierarchy == true)
        {
            yield return null;
        }
    }

    // Let's make a function to play a video corresponding to the randomized trial
	public IEnumerator playVideo()
	{
        if (homeArray[0] == rightNorth)
        {
            if (outBoundArray[0] == rightNorth)
            {
                videoPlayerOne.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerOne.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerOne.texture;
                canvasVideo.SetActive(true);
                videoPlayerOne.Play();
            }
            else if (outBoundArray[0] == rightSouth)
            {
                videoPlayerTwo.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerTwo.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerTwo.texture;
                canvasVideo.SetActive(true);
                videoPlayerTwo.Play();
            }
            else if (outBoundArray[0] == leftNorth)
            {
                videoPlayerThree.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerThree.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerThree.texture;
                canvasVideo.SetActive(true);
                videoPlayerThree.Play();
            }
            else
            {
                videoPlayerFour.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerFour.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerFour.texture;
                canvasVideo.SetActive(true);
                videoPlayerFour.Play();
            }
        }
        else if (homeArray[0] == rightSouth)
        {
            if (outBoundArray[0] == rightNorth)
            {
                videoPlayerFive.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerFive.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerFive.texture;
                canvasVideo.SetActive(true);
                videoPlayerFive.Play();
            }
            else if (outBoundArray[0] == rightSouth)
            {
                videoPlayerSix.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerSix.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerSix.texture;
                canvasVideo.SetActive(true);
                videoPlayerSix.Play();
            }
            else if (outBoundArray[0] == leftNorth)
            {
                videoPlayerSeven.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerSeven.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerSeven.texture;
                canvasVideo.SetActive(true);
                videoPlayerSeven.Play();
            }
            else
            {
                videoPlayerEight.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerEight.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerEight.texture;
                canvasVideo.SetActive(true);
                videoPlayerEight.Play();
            }
        }
        else if (homeArray[0] == leftNorth)
        {
            if (outBoundArray[0] == rightNorth)
            {
                videoPlayerNine.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerNine.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerNine.texture;
                canvasVideo.SetActive(true);
                videoPlayerNine.Play();
            }
            else if (outBoundArray[0] == rightSouth)
            {
                videoPlayerTen.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerTen.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerTen.texture;
                canvasVideo.SetActive(true);
                videoPlayerTen.Play();
            }
            else if (outBoundArray[0] == leftNorth)
            {
                videoPlayerEleven.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerEleven.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerEleven.texture;
                canvasVideo.SetActive(true);
                videoPlayerEleven.Play();
            }
            else
            {
                videoPlayerTwelve.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerTwelve.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerTwelve.texture;
                canvasVideo.SetActive(true);
                videoPlayerTwelve.Play();
            }
        }
        else
        {
            if (outBoundArray[0] == rightNorth)
            {
                videoPlayerThirteen.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerThirteen.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerThirteen.texture;
                canvasVideo.SetActive(true);
                videoPlayerThirteen.Play();
            }
            else if (outBoundArray[0] == rightSouth)
            {
                videoPlayerFourteen.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerFourteen.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerFourteen.texture;
                canvasVideo.SetActive(true);
                videoPlayerFourteen.Play();
            }
            else if (outBoundArray[0] == leftNorth)
            {
                videoPlayerFifteen.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerFifteen.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerFifteen.texture;
                canvasVideo.SetActive(true);
                videoPlayerFifteen.Play();
            }
            else
            {
                videoPlayerSixteen.Prepare();
                WaitForSeconds waitForSeconds = new WaitForSeconds(1);
                while (!videoPlayerSixteen.isPrepared)
                {
                    yield return waitForSeconds;
                    break;
                }
                rawImage.texture = videoPlayerSixteen.texture;
                canvasVideo.SetActive(true);
                videoPlayerSixteen.Play();
            }
        }
    }

    public IEnumerator videoCaptures()
    {
        trialCount = 0;
        yield return StartCoroutine(axisChecker());
        yield return StartCoroutine(waitForTrigger());
        resetPost.SetActive(true);
        resetPostWhite.SetActive(true);
        yield return StartCoroutine(axisChecker());
        yield return StartCoroutine(waitForTrigger());
        for (int i = 0; i < homeArray.Length; i++)
        {
			homeCode = i;
            for (int j = 0; j < outBoundArray.Length; j++)
            {
				outCode = j;
                trialCount = trialCount + 1;
                tree.SetActive(true);
                tower.SetActive(true);
                rock.SetActive(true);
                resetPost.SetActive(false);
                resetPostWhite.SetActive(false);
                if (homeArray[i] == rightNorth)
                {
                    homePost.transform.position = homeRight;
                    rotateHomeNorthR();
                }
                else if (homeArray[i] == rightSouth)
                {
                    homePost.transform.position = homeRight;
                    rotateHomeSouthR();
                }
                else if (homeArray[i] == leftNorth)
                {
                    homePost.transform.position = homeLeft;
                    rotateHomeNorthL();
                }
                else
                {
                    homePost.transform.position = homeLeft;
                    rotateHomeSouthL();
                }
                if (outBoundArray[j] == rightNorth)
                {
                    outBoundPost.transform.position = outBoundRight;
                    rotateOutNorthR();
                }
                else if (outBoundArray[j] == rightSouth)
                {
                    outBoundPost.transform.position = outBoundRight;
                    rotateOutSouthR();
                }
                else if (outBoundArray[j] == leftNorth)
                {
                    outBoundPost.transform.position = outBoundLeft;
                    rotateOutNorthL();
                }
                else
                {
                    outBoundPost.transform.position = outBoundLeft;
                    rotateOutSouthL();
                }
                homePost.SetActive(true);
                yield return StartCoroutine(arriveAtHomePost());
                outBoundPost.SetActive(true);
                outBoundPostLoc = outBoundPost.transform.position;
                yield return StartCoroutine(arriveAtOutPost());
                outBoundPost.transform.position = outBoundPostFINLoc;
                outBoundPost.SetActive(true);
                yield return StartCoroutine(arriveAtOutPost());
                yield return StartCoroutine(axisChecker());
                yield return StartCoroutine(waitForTrigger());
                tree.SetActive(false);
                tower.SetActive(false);
                rock.SetActive(false);
                if (trialCount < 16)
                {
                    resetPost.SetActive(true);
                    resetPostWhite.SetActive(true);
                }
                yield return StartCoroutine(axisChecker());
                yield return StartCoroutine(waitForTrigger());
                if (trialCount == 16)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    }

    // Main experiment code
    public IEnumerator startExp()
    {
        // Print initial data and have participant press button to start exp
        CreateText();
		yield return StartCoroutine (axisChecker ());
		yield return StartCoroutine (waitForTrigger ());
        resetPost.SetActive(true);
        resetPostWhite.SetActive(true);
		yield return StartCoroutine(axisChecker());
		yield return StartCoroutine(waitForTrigger());

        // loop through blocks
        {
            for (int i = 0; i < nBlocks; i++)
            {
                // randomize trials and count start arrays
                for (int j = 0; j < trialArray.Length; j++)
                {
                    int trialNum = trialArray[j];
                    int randomTrial = Random.Range(0, trialArray.Length);
                    trialArray[j] = trialArray[randomTrial];
                    trialArray[randomTrial] = trialNum;
                }
                for (int c = 0; c < countStartArr.Length; c++)
                {
                    int countNum = countStartArr[c];
                    int randomCount = Random.Range(0, countStartArr.Length);
                    countStartArr[c] = countStartArr[randomCount];
                    countStartArr[randomCount] = countNum;
                }
                for (int k = 0; k < trialArray.Length; k++)
                {
                    for (int l = 0; l < homeArray.Length; l++)
                    {
                        int homeNum = homeArray[l];
                        int randomHome = Random.Range(0, homeArray.Length);
                        homeArray[l] = homeArray[randomHome];
                        homeArray[randomHome] = homeNum;
                    }
                    for(int o = 0; o < outBoundArray.Length; o++)
                    {
                        int outBoundNum = outBoundArray[o];
                        int randomOutBound = Random.Range(0, outBoundArray.Length);
                        outBoundArray[o] = outBoundArray[randomOutBound];
                        outBoundArray[randomOutBound] = outBoundNum;
                    }

                    // Determine trial type and set objects active
                    trialType = trialArray[k];
                    tree.SetActive(true);
                    tower.SetActive(true);
                    rock.SetActive(true);
                    resetPost.SetActive(false);
                    resetPostWhite.SetActive(false);
                    trialCount = trialCount + 1;

                    // Trial code by trial type
                    // landmarks only and combination conditions here
                    if (trialArray[k] == landOnly || trialArray[k] == combCue)
                    {
                        if (trialArray[k] == landOnly)
                        {
                            trialType = landOnly;
                        }
                        else
                        {
                            trialType = combCue;
                        }

                        // place home location
                        if (homeArray[0] == rightNorth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeNorthR();
                            homeCode = 0;
                        }
                        else if (homeArray[0] == rightSouth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeSouthR();
                            homeCode = 1;
                        }
                        else if (homeArray[0] == leftNorth)
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeNorthL();
                            homeCode = 2;
                        }
                        else
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeSouthL();
                            homeCode = 3;
                        }

                        // place outbound post
                        if (outBoundArray[0] == rightNorth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutNorthR();
                            outCode = 0;
                        }
                        else if (outBoundArray[0] == rightSouth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutSouthR();
                            outCode = 1;
                        }
                        else if (outBoundArray[0] == leftNorth)
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutNorthL();
                            outCode = 2;
                        }
                        else
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutSouthL();
                            outCode = 3;
                        }

                        // Run through trial code
                        // Start with both present condtions (also works for one present combination)
                        if (bothPresentCondition || !bothPresentCondition && trialType == combCue)
                        {
                            homePost.SetActive(true);
                            yield return StartCoroutine(arriveAtHomePost());
                            outBoundPost.SetActive(true);
							outBoundPostLoc = outBoundPost.transform.position;
                            yield return StartCoroutine(arriveAtOutPost());
                            outBoundPost.transform.position = outBoundPostFINLoc;
                            outBoundPost.SetActive(true);
                            yield return StartCoroutine(arriveAtOutPost());
                            canvasBlack.SetActive(true);        // Participants count backwards here
                            groundPlane.SetActive(false);
                            if (countStartArr[k] == fifteenStart)
                            {
                                FifteenStart.SetActive(true);
                            }
                            else if (countStartArr[k] == thirtyStart)
                            {
                                ThirtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == fortyFiveStart)
                            {
                                FortyFiveStart.SetActive(true);
                            }
                            else if (countStartArr[k] == sixtyStart)
                            {
                                SixtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == seventyFiveStart)
                            {
                                SeventyFiveStart.SetActive(true);
                            }
                            else
                            {
                                NinetyStart.SetActive(true);
                            }
                            yield return StartCoroutine(axisChecker());
                            yield return StartCoroutine(waitForTrigger());
                            FifteenStart.SetActive(false);
                            ThirtyStart.SetActive(false);
                            FortyFiveStart.SetActive(false);
                            SixtyStart.SetActive(false);
                            SeventyFiveStart.SetActive(false);
                            NinetyStart.SetActive(false);
                            canvasBlack.SetActive(false);
                            groundPlane.SetActive(true);

                            // Begin return path
                            startTime = Time.time;
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            responseTime = Time.time - startTime;
                            CreateText();
                            tree.SetActive(false);
                            tower.SetActive(false);
                            rock.SetActive(false);

                            // begin next trial or end experiment by pressing button if last trial
                            if (trialCount < 44)
                            {
                                resetPost.SetActive(true);
                                resetPostWhite.SetActive(true);
                            }
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            if (trialCount == 44)
                            {
                                UnityEditor.EditorApplication.isPlaying = false;
                            }
                        }

                        // if landmark only condition and one present
                        else
                        {
                            canvasBlack.SetActive(true);    // Keep this on in background while video plays
							groundPlane.SetActive(false);
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            StartCoroutine(playVideo());
                            yield return StartCoroutine(axisChecker());
                            yield return StartCoroutine(waitForTrigger());
                            canvasVideo.SetActive(false);           // Participants count backwards here
                            if (countStartArr[k] == fifteenStart)
                            {
                                FifteenStart.SetActive(true);
                            }
                            else if (countStartArr[k] == thirtyStart)
                            {
                                ThirtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == fortyFiveStart)
                            {
                                FortyFiveStart.SetActive(true);
                            }
                            else if (countStartArr[k] == sixtyStart)
                            {
                                SixtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == seventyFiveStart)
                            {
                                SeventyFiveStart.SetActive(true);
                            }
                            else
                            {
                                NinetyStart.SetActive(true);
                            }
                            yield return StartCoroutine(axisChecker());         
                            yield return StartCoroutine(waitForTrigger());
                            FifteenStart.SetActive(false);
                            ThirtyStart.SetActive(false);
                            FortyFiveStart.SetActive(false);
                            SixtyStart.SetActive(false);
                            SeventyFiveStart.SetActive(false);
                            NinetyStart.SetActive(false);
                            canvasBlack.SetActive(false);
							groundPlane.SetActive (true);

                            // start return path
                            startTime = Time.time;
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            responseTime = Time.time - startTime;
                            CreateText();
                            tree.SetActive(false);
                            tower.SetActive(false);
                            rock.SetActive(false);
                            if (trialCount < 44)
                            {
                                resetPost.SetActive(true);
                                resetPostWhite.SetActive(true);
                            }
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            if (trialCount == 44)
                            {
                                UnityEditor.EditorApplication.isPlaying = false;
                            }
                        }
                    }

                    // self motion only condition
                    else if (trialArray[k] == selfOnly)
                    {
                        trialType = selfOnly;
                        if (homeArray[0] == rightNorth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeNorthR();
                            homeCode = 0;
                        }
                        else if (homeArray[0] == rightSouth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeSouthR();
                            homeCode = 1;
                        }
                        else if (homeArray[0] == leftNorth)
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeNorthL();
                            homeCode = 2;
                        }
                        else
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeSouthL();
                            homeCode = 3;
                        }
                        if (outBoundArray[0] == rightNorth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutNorthR();
                            outCode = 0;
                        }
                        else if (outBoundArray[0] == rightSouth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutSouthR();
                            outCode = 1;
                        }
                        else if (outBoundArray[0] == leftNorth)
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutNorthL();
                            outCode = 2;
                        }
                        else
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutSouthL();
                            outCode = 3;
                        }

                        // self motion both present
                        if (bothPresentCondition)
                        {
                            homePost.SetActive(true);
                            yield return StartCoroutine(arriveAtHomePost());
                            outBoundPost.SetActive(true);
							outBoundPostLoc = outBoundPost.transform.position;
                            yield return StartCoroutine(arriveAtOutPost());
                            outBoundPost.transform.position = outBoundPostFINLoc;
                            outBoundPost.SetActive(true);
                            yield return StartCoroutine(arriveAtOutPost());
                            canvasBlack.SetActive(true);            // Participants count backwards and are disoriented here
                            groundPlane.SetActive(false);
                            if (countStartArr[k] == fifteenStart)
                            {
                                FifteenStart.SetActive(true);
                            }
                            else if (countStartArr[k] == thirtyStart)
                            {
                                ThirtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == fortyFiveStart)
                            {
                                FortyFiveStart.SetActive(true);
                            }
                            else if (countStartArr[k] == sixtyStart)
                            {
                                SixtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == seventyFiveStart)
                            {
                                SeventyFiveStart.SetActive(true);
                            }
                            else
                            {
                                NinetyStart.SetActive(true);
                            }
                            yield return StartCoroutine(axisChecker());
                            yield return StartCoroutine(waitForTrigger());
                            FifteenStart.SetActive(false);
                            ThirtyStart.SetActive(false);
                            FortyFiveStart.SetActive(false);
                            SixtyStart.SetActive(false);
                            SeventyFiveStart.SetActive(false);
                            NinetyStart.SetActive(false);
                            canvasBlack.SetActive(false);
                            groundPlane.SetActive(true);

                            // begin return path
                            startTime = Time.time;
                            tree.SetActive(false);
                            tower.SetActive(false);
                            rock.SetActive(false);
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            responseTime = Time.time - startTime;
                            CreateText();
                            if (trialCount < 44)
                            {
                                resetPost.SetActive(true);
                                resetPostWhite.SetActive(true);
                            }
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            if (trialCount == 44)
                            {
                                UnityEditor.EditorApplication.isPlaying = false;
                            }
                        }

                        // self motion one present
                        else
                        {
                            canvasBlack.SetActive(true);
							groundPlane.SetActive (false);
                            tree.SetActive(false);
                            tower.SetActive(false);
                            rock.SetActive(false);
							yield return StartCoroutine (axisChecker ());               // Have participants count backwards here
							yield return StartCoroutine (waitForTrigger ());
                            if (countStartArr[k] == fifteenStart)
                            {
                                FifteenStart.SetActive(true);
                            }
                            else if (countStartArr[k] == thirtyStart)
                            {
                                ThirtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == fortyFiveStart)
                            {
                                FortyFiveStart.SetActive(true);
                            }
                            else if (countStartArr[k] == sixtyStart)
                            {
                                SixtyStart.SetActive(true);
                            }
                            else if (countStartArr[k] == seventyFiveStart)
                            {
                                SeventyFiveStart.SetActive(true);
                            }
                            else
                            {
                                NinetyStart.SetActive(true);
                            }
                            yield return StartCoroutine(axisChecker());
                            yield return StartCoroutine(waitForTrigger());
                            FifteenStart.SetActive(false);
                            ThirtyStart.SetActive(false);
                            FortyFiveStart.SetActive(false);
                            SixtyStart.SetActive(false);
                            SeventyFiveStart.SetActive(false);
                            NinetyStart.SetActive(false);
                            startTime = Time.time;
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            responseTime = Time.time - startTime;
                            CreateText();
                            canvasBlack.SetActive(false);
							groundPlane.SetActive (true);
                            if (trialCount < 44)
                            {
                                resetPost.SetActive(true);
                                resetPostWhite.SetActive(true);
                            }
							yield return StartCoroutine (axisChecker ());
							yield return StartCoroutine (waitForTrigger ());
                            if (trialCount == 44)
                            {
                                UnityEditor.EditorApplication.isPlaying = false;
                            }
                        }
                    }

                    // conflict trial
                    else if (trialArray[k] == confCue)
                    {
                        trialType = confCue;
                        if (homeArray[0] == rightNorth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeNorthR();
                            homeCode = 0;
                        }
                        else if (homeArray[0] == rightSouth)
                        {
                            homePost.transform.position = homeRight;
                            rotateHomeSouthR();
                            homeCode = 1;
                        }
                        else if (homeArray[0] == leftNorth)
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeNorthL();
                            homeCode = 2;
                        }
                        else
                        {
                            homePost.transform.position = homeLeft;
                            rotateHomeSouthL();
                            homeCode = 3;
                        }
                        if (outBoundArray[0] == rightNorth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutNorthR();
                            outCode = 0;
                        }
                        else if (outBoundArray[0] == rightSouth)
                        {
                            outBoundPost.transform.position = outBoundRight;
                            rotateOutSouthL();
                            outCode = 1;
                        }
                        else if (outBoundArray[0] == leftNorth)
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutNorthL();
                            outCode = 2;
                        }
                        else
                        {
                            outBoundPost.transform.position = outBoundLeft;
                            rotateOutSouthL();
                            outCode = 3;
                        }
                        homePost.SetActive(true);
                        yield return StartCoroutine(arriveAtHomePost());
                        outBoundPost.SetActive(true);
						outBoundPostLoc = outBoundPost.transform.position;				
                        yield return StartCoroutine(arriveAtOutPost());
                        outBoundPost.transform.position = outBoundPostFINLoc;
                        outBoundPost.SetActive(true);
                        yield return StartCoroutine(arriveAtOutPost());
                        canvasBlack.SetActive(true);            // Participants count backwards here
                        groundPlane.SetActive(false);
                        if (countStartArr[k] == fifteenStart)
                        {
                            FifteenStart.SetActive(true);
                        }
                        else if (countStartArr[k] == thirtyStart)
                        {
                            ThirtyStart.SetActive(true);
                        }
                        else if (countStartArr[k] == fortyFiveStart)
                        {
                            FortyFiveStart.SetActive(true);
                        }
                        else if (countStartArr[k] == sixtyStart)
                        {
                            SixtyStart.SetActive(true);
                        }
                        else if (countStartArr[k] == seventyFiveStart)
                        {
                            SeventyFiveStart.SetActive(true);
                        }
                        else
                        {
                            NinetyStart.SetActive(true);
                        }
                        yield return StartCoroutine(axisChecker());
                        yield return StartCoroutine(waitForTrigger());
                        FifteenStart.SetActive(false);
                        ThirtyStart.SetActive(false);
                        FortyFiveStart.SetActive(false);
                        SixtyStart.SetActive(false);
                        SeventyFiveStart.SetActive(false);
                        NinetyStart.SetActive(false);
                        canvasBlack.SetActive(false);
                        groundPlane.SetActive(true);

                        // start return path
                        startTime = Time.time;
                        outBoundPost.SetActive(false);
                        // Rotate Landmarks here
                        rotateLandmarks();
						yield return StartCoroutine (axisChecker ());
						yield return StartCoroutine (waitForTrigger ());
                        responseTime = Time.time - startTime;
                        CreateText();
                        tree.SetActive(false);
                        tower.SetActive(false);
                        rock.SetActive(false);
                        // undo landmark rotation
                        undoRotate();
                        if (trialCount < 44)
                        {
                            resetPost.SetActive(true);
                            resetPostWhite.SetActive(true);
                        }
						yield return StartCoroutine (axisChecker ());
						yield return StartCoroutine (waitForTrigger ());
                        if (trialCount == 44)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                    }
                }
            }
        }
    }

	// Use this for initialization
	void Start () {

		// Participant info and response vectors
		playerPos = player.transform.position;
		pNum = 999;
		trialCount = 0;
		if (pNum % 2 == 0) {
			bothPresentCondition = true;
		} else {
			bothPresentCondition = false;
		}

		// Get post location vectors
		homePostLoc = homePost.transform.position;
		outBoundPostLoc = outBoundPost.transform.position;
		outBoundPostFINLoc = new Vector3 (0f, 0f, .0f);
		homeRight = new Vector3 (2.15f, 0f, .0f);
		homeLeft = new Vector3 (-2.15f, 0f, .0f);
		outBoundRight = new Vector3 (1.3f, 0f, .0f);
		outBoundLeft = new Vector3 (-1.3f, 0f, .0f);

		// Set up trial, path, and count arrays
		trialArray = new int[4];
		trialArray [0] = landOnly;
		trialArray [1] = selfOnly;
		trialArray [2] = combCue;
		trialArray [3] = confCue;
		homeArray = new int[4];
		homeArray [0] = rightNorth;
		homeArray [1] = rightSouth;
		homeArray [2] = leftNorth;
		homeArray [3] = leftSouth;
		outBoundArray = new int[4];
		outBoundArray [0] = rightNorth;
		outBoundArray [1] = rightSouth;
		outBoundArray [2] = leftNorth;
		outBoundArray [3] = leftSouth;
        countStartArr = new int[6];
        countStartArr[0] = fifteenStart;
        countStartArr[1] = thirtyStart;
        countStartArr[2] = fortyFiveStart;
        countStartArr[3] = sixtyStart;
        countStartArr[4] = seventyFiveStart;
        countStartArr[5] = ninetyStart;

		// Set everything inactive at start
		outBoundPost.SetActive (false);
		resetPost.SetActive (false);
		resetPostWhite.SetActive (false);
		homePost.SetActive (false);
		rock.SetActive (false);
		tower.SetActive (false);
		tree.SetActive (false);
		canvasBlack.SetActive (false);
		canvasVideo.SetActive (false);
        FifteenStart.SetActive(false);
        ThirtyStart.SetActive(false);
        FortyFiveStart.SetActive(false);
        SixtyStart.SetActive(false);
        SeventyFiveStart.SetActive(false);
        NinetyStart.SetActive(false);
		colliderSphere.GetComponent<Renderer> ().enabled = false;
		homePostCollider.GetComponent<Renderer> ().enabled = false;
		outPostCollider.GetComponent<Renderer> ().enabled = false;
		headingTracker.GetComponent<Renderer> ().enabled = false;

		// Enable this for testing of canvas video and canvas black; ground needs to disappears as to not cut off canvas
		groundPlane.SetActive(true);

        // Start Experiment
        StartCoroutine(startExp());
        //StartCoroutine(videoCaptures());
		//StartCoroutine(playVideo());
	}
    void Update()
    {
        playerPos = player.transform.position;
		homePostLoc = homePost.transform.position;
    }
}

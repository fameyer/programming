# by Falk Meyer, 2021-03-20

class AnalysisCase:
    """
    Base class for all analysis test cases
    """

    def __init__(self):
        pass

    def analyze(self, *props):
        """
        Do the analysis
        :return:
        """
        raise NotImplementedError()

# by Falk Meyer, 2021-03-21

# imports
# =======================
# system
import os
import sys
# third-party
import geopandas
# local
from analysisUtils.AnalysisPlotter import AnalysisPlotter
from analysisUtils.SpatialAnalysisCase import SpatialAnalysisCase

# constants
# =======================
dataFolder = './data'
buildingsPath = os.path.join(dataFolder, 'buildings')
buildingsIdField = 'osm_id'
tunnelPath = os.path.join(dataFolder, 'tunnel')
highwayPath = os.path.join(dataFolder, 'highway')
projCrs = 32632

# logic
# ======================

def main():
    """
    Main function call
    """
    # test props
    inputBuffer = 350
    bufferStyle = 2

    try:
        # read data
        print('Reading data...')
        buildingFrame = geopandas.read_file(buildingsPath)
        buildingCrs = buildingFrame.crs
        tunnelFrame = geopandas.read_file(tunnelPath)
        highwayFrame = geopandas.read_file(highwayPath)

        # project frames from geographic to projected crs
        print('Projecting data to fitting coordinate system...')
        buildingFrame.to_crs(epsg=projCrs, inplace=True)
        tunnelFrame.to_crs(epsg=projCrs, inplace=True)
        highwayFrame.to_crs(epsg=projCrs, inplace=True)

        # tunneled part of the highway
        highwayTunnelFrame = geopandas.overlay(highwayFrame, tunnelFrame, how='intersection')

        # do a spatial analysis
        print('Starting spatial analysis...')
        spatialAnalysis = SpatialAnalysisCase(buildingFrame, buildingsIdField, highwayTunnelFrame)
        affectedBuildings = spatialAnalysis.analyze(inputBuffer, bufferStyle)

        # create plot
        print('Plotting the analysis...')
        plotter = AnalysisPlotter()
        plotter.plot(affectedBuildings, highwayTunnelFrame['geometry'].buffer(inputBuffer, cap_style=bufferStyle),
                     highwayTunnelFrame, fileName='target/AnalysisReport.pdf', table=True)

        # write result to file with original crs
        print('Writing analysis shape file to target folder...')
        affectedBuildings.to_crs(buildingCrs).to_file('./target/affectedBuildings')

        print('Successfully finished')

    except:
        print('Aborted due to error: ', sys.exc_info()[0])


if __name__ == '__main__':
    main()
